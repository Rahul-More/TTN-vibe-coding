import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ConstructionOutlinedIcon from '@mui/icons-material/ConstructionOutlined';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Container from '@mui/material/Container';
import Paper from '@mui/material/Paper';
import Typography from '@mui/material/Typography';
import { Link as RouterLink, useParams } from 'react-router-dom';

export function TicketDetailPlaceholder() {
  const { id } = useParams<{ id: string }>();

  return (
    <Container maxWidth="sm">
      <Button
        component={RouterLink}
        to="/tickets"
        startIcon={<ArrowBackIcon />}
        sx={{ mb: 3, color: 'text.secondary' }}
      >
        Back to tickets
      </Button>

      <Paper
        elevation={0}
        sx={{
          p: { xs: 4, md: 6 },
          textAlign: 'center',
          border: '1px solid',
          borderColor: 'divider',
          borderRadius: 3,
        }}
      >
        <Box
          sx={{
            width: 80,
            height: 80,
            mx: 'auto',
            mb: 3,
            borderRadius: '50%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            bgcolor: 'primary.main',
            color: 'primary.contrastText',
            opacity: 0.9,
          }}
        >
          <ConstructionOutlinedIcon sx={{ fontSize: 40 }} />
        </Box>
        <Typography variant="h5" gutterBottom>
          Ticket detail coming soon
        </Typography>
        <Typography color="text.secondary" sx={{ mb: 3 }}>
          Ticket #{id} detail view — including edits, status changes, and comments — will be
          implemented in the next phase.
        </Typography>
        <Button component={RouterLink} to="/tickets" variant="contained">
          Return to ticket list
        </Button>
      </Paper>
    </Container>
  );
}
