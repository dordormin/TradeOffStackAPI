import React from 'react';
import type {Props} from '@theme/Logo';
import Link from '@docusaurus/Link';

export default function LogoWrapper(props: Props): JSX.Element {
  return (
    <Link to="/" className={`flex items-center gap-2.5 navbar__title ${props.className || ''}`}>
      {/* SVG Icon representing a "Stack" */}
      <svg 
        width="32" 
        height="32" 
        viewBox="0 0 32 32" 
        fill="none" 
        xmlns="http://www.w3.org/2000/svg" 
        className="shrink-0"
      >
        <path d="M16 3L3 9.5L16 16L29 9.5L16 3Z" fill="var(--ifm-color-primary)" />
        <path d="M3 15.5L16 22L29 15.5" stroke="var(--ifm-color-primary)" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" style={{ opacity: 0.7 }} />
        <path d="M3 21.5L16 28L29 21.5" stroke="var(--ifm-color-primary)" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round" style={{ opacity: 0.4 }} />
      </svg>
      
      {/* Brand Text */}
      <div className="flex flex-col ml-2">
        <span className="text-xl tracking-tight text-foreground font-extrabold uppercase font-mono leading-none" style={{ color: 'var(--ifm-color-content)', fontSize: '1.25rem', lineHeight: 1 }}>
          TradeOff<span style={{ color: 'var(--ifm-color-primary)', fontWeight: 900 }}>Stack</span>
        </span>
      </div>
    </Link>
  );
}
