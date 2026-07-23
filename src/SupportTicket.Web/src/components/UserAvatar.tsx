import Avatar from '@mui/material/Avatar';
import { getInitials } from '../utils/labels';

interface UserAvatarProps {
  name: string;
  size?: number;
}

const avatarColors = ['#4f46e5', '#0ea5e9', '#8b5cf6', '#ec4899', '#f59e0b'];

function colorForName(name: string): string {
  let hash = 0;
  for (let i = 0; i < name.length; i++) {
    hash = name.charCodeAt(i) + ((hash << 5) - hash);
  }
  return avatarColors[Math.abs(hash) % avatarColors.length];
}

export function UserAvatar({ name, size = 32 }: UserAvatarProps) {
  return (
    <Avatar
      sx={{
        width: size,
        height: size,
        fontSize: size * 0.38,
        fontWeight: 700,
        bgcolor: colorForName(name),
      }}
    >
      {getInitials(name)}
    </Avatar>
  );
}
