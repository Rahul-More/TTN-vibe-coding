import ConfirmationNumberOutlinedIcon from '@mui/icons-material/ConfirmationNumberOutlined';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Container from '@mui/material/Container';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import type { ReactNode } from 'react';
import { Link as RouterLink, useLocation } from 'react-router-dom';

interface AppLayoutProps {
  children: ReactNode;
}

export function AppLayout({ children }: AppLayoutProps) {
  const location = useLocation();
  const isList = location.pathname === '/tickets';

  return (
    <Box sx={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      <AppBar
        position="sticky"
        elevation={0}
        sx={{
          background: 'linear-gradient(135deg, #312e81 0%, #4f46e5 55%, #6366f1 100%)',
          borderBottom: '1px solid rgba(255,255,255,0.12)',
        }}
      >
        <Container maxWidth="lg">
          <Toolbar disableGutters sx={{ py: 0.5, gap: 2 }}>
            <Box
              component={RouterLink}
              to="/tickets"
              sx={{
                display: 'flex',
                alignItems: 'center',
                gap: 1.5,
                textDecoration: 'none',
                color: 'inherit',
                flexGrow: 1,
              }}
            >
              <Box
                sx={{
                  width: 40,
                  height: 40,
                  borderRadius: 2,
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  bgcolor: 'rgba(255,255,255,0.15)',
                  backdropFilter: 'blur(8px)',
                }}
              >
                <ConfirmationNumberOutlinedIcon />
              </Box>
              <Box>
                <Typography variant="h6" sx={{ lineHeight: 1.2, fontWeight: 700 }}>
                  Support Desk
                </Typography>
                <Typography variant="caption" sx={{ opacity: 0.85 }}>
                  Ticket Management
                </Typography>
              </Box>
            </Box>

            <Box
              component={RouterLink}
              to="/tickets"
              sx={{
                px: 2,
                py: 1,
                borderRadius: 2,
                textDecoration: 'none',
                color: 'white',
                fontWeight: 600,
                fontSize: '0.875rem',
                bgcolor: isList ? 'rgba(255,255,255,0.2)' : 'transparent',
                '&:hover': { bgcolor: 'rgba(255,255,255,0.12)' },
              }}
            >
              Tickets
            </Box>
          </Toolbar>
        </Container>
      </AppBar>

      <Box
        component="main"
        sx={{
          flexGrow: 1,
          py: { xs: 3, md: 4 },
          background:
            'radial-gradient(ellipse at top, rgba(99,102,241,0.08) 0%, transparent 55%), #f1f5f9',
        }}
      >
        {children}
      </Box>

      <Box
        component="footer"
        sx={{
          py: 2,
          textAlign: 'center',
          color: 'text.secondary',
          fontSize: '0.8rem',
          borderTop: '1px solid',
          borderColor: 'divider',
          bgcolor: 'background.paper',
        }}
      >
        Support Ticket Management System
      </Box>
    </Box>
  );
}
