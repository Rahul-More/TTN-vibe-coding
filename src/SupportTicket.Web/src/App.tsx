import { Navigate, Route, Routes } from 'react-router-dom';
import { AppLayout } from './components/layout/AppLayout';
import { CreateTicketPage } from './pages/CreateTicketPage';
import { TicketDetailPlaceholder } from './pages/TicketDetailPlaceholder';
import { TicketListPage } from './pages/TicketListPage';

export default function App() {
  return (
    <AppLayout>
      <Routes>
        <Route path="/" element={<Navigate to="/tickets" replace />} />
        <Route path="/tickets" element={<TicketListPage />} />
        <Route path="/tickets/new" element={<CreateTicketPage />} />
        <Route path="/tickets/:id" element={<TicketDetailPlaceholder />} />
      </Routes>
    </AppLayout>
  );
}
