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
}

export function PriorityChip({ priority }: PriorityChipProps) {
  const style = priorityStyles[priority];
  return (
    <Chip
      label={priority}
      size="small"
      sx={{
        bgcolor: style.bgcolor,
        color: style.color,
        border: '1px solid',
        borderColor: style.borderColor,
        fontWeight: 600,
      }}
    />
  );
}
