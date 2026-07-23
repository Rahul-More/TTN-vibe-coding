import Chip from '@mui/material/Chip';
import type { TicketPriority } from '../types/api';

const priorityStyles: Record<
  TicketPriority,
  { bgcolor: string; color: string; borderColor: string }
> = {
  Low: {
    bgcolor: '#ecfdf5',
    color: '#047857',
    borderColor: '#a7f3d0',
  },
  Medium: {
    bgcolor: '#fffbeb',
    color: '#b45309',
    borderColor: '#fde68a',
  },
  High: {
    bgcolor: '#fef2f2',
    color: '#b91c1c',
    borderColor: '#fecaca',
  },
};

interface PriorityChipProps {
  priority: TicketPriority;
  /** Outline style for unselected priority options — keeps text fully readable */
  variant?: 'filled' | 'outline';
}

export function PriorityChip({ priority, variant = 'filled' }: PriorityChipProps) {
  const style = priorityStyles[priority];
  const isOutline = variant === 'outline';

  return (
    <Chip
      label={priority}
      size="small"
      sx={{
        bgcolor: isOutline ? '#ffffff' : style.bgcolor,
        color: style.color,
        border: '1.5px solid',
        borderColor: style.borderColor,
        fontWeight: 700,
        fontSize: '0.8125rem',
        letterSpacing: '0.01em',
        '& .MuiChip-label': {
          px: 1.25,
        },
      }}
    />
  );
}
