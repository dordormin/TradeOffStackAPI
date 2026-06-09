import React, { createContext, useContext, useState, useEffect } from 'react';
import type { User, UserRole } from '@/types';
import { apiClient } from '@/api/apiClient';

interface AuthState {
  isAuthenticated: boolean;
  role: UserRole | null;
  token: string | null;
  userId: string | null;
  user: User | null;
  isLoading: boolean;
}

interface AuthContextType extends AuthState {
  login: (token: string, role: UserRole, userId: string) => Promise<void>;
  logout: () => void;
  refreshUser: () => Promise<void>;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [authState, setAuthState] = useState<AuthState>({
    isAuthenticated: false,
    role: null,
    token: null,
    userId: null,
    user: null,
    isLoading: true,
  });

  const fetchUserProfile = async (userId: string, token: string) => {
    try {
      const response = await apiClient.get<User>(`/user/${userId}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      return response.data;
    } catch (err) {
      console.error('Failed to fetch user profile during initialization', err);
      return null;
    }
  };

  useEffect(() => {
    const initAuth = async () => {
      const token = localStorage.getItem('jwt_token');
      const role = localStorage.getItem('user_role') as UserRole | null;
      const userId = localStorage.getItem('user_id');

      if (token && role && userId) {
        const userDetails = await fetchUserProfile(userId, token);
        setAuthState({
          isAuthenticated: true,
          role,
          token,
          userId,
          user: userDetails,
          isLoading: false,
        });
      } else {
        setAuthState({
          isAuthenticated: false,
          role: null,
          token: null,
          userId: null,
          user: null,
          isLoading: false,
        });
      }
    };

    initAuth();

    // Global listener for 401 Unauthorized from Axios
    const handleUnauthorized = () => {
      logout();
    };

    window.addEventListener('auth:unauthorized', handleUnauthorized);
    return () => window.removeEventListener('auth:unauthorized', handleUnauthorized);
  }, []);

  const login = async (token: string, role: UserRole, userId: string) => {
    localStorage.setItem('jwt_token', token);
    localStorage.setItem('user_role', role);
    localStorage.setItem('user_id', userId);

    const userDetails = await fetchUserProfile(userId, token);

    setAuthState({
      isAuthenticated: true,
      role,
      token,
      userId,
      user: userDetails,
      isLoading: false,
    });
  };

  const logout = () => {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('user_role');
    localStorage.removeItem('user_id');
    setAuthState({
      isAuthenticated: false,
      role: null,
      token: null,
      userId: null,
      user: null,
      isLoading: false,
    });
  };

  const refreshUser = async () => {
    if (authState.userId && authState.token) {
      try {
        const response = await apiClient.get<User>(`/user/${authState.userId}`);
        setAuthState((prev) => ({
          ...prev,
          user: response.data,
        }));
      } catch (err) {
        console.error('Failed to refresh user profile', err);
      }
    }
  };

  return (
    <AuthContext.Provider value={{ ...authState, login, logout, refreshUser }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
