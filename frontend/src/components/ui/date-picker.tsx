import { cn } from '@/lib/utils';

export interface DatePickerProps {
  value?: string;
  onChange?: (date: string) => void;
  className?: string;
  placeholder?: string;
  disabled?: boolean;
}

export function DatePicker({ value, onChange, className, disabled }: DatePickerProps) {
  return (
    <input
      type="date"
      value={value || ''}
      onChange={(e) => onChange && onChange(e.target.value)}
      disabled={disabled}
      className={cn(
        "flex min-h-[38px] w-full rounded-md border border-border bg-background px-3 py-2 text-sm transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-primary disabled:cursor-not-allowed disabled:opacity-50 cursor-pointer",
        className
      )}
    />
  );
}
