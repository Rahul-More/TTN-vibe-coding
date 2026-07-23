const STORAGE_KEY = 'supportTicket.createdBy';

export function getStoredCreatedBy(): number | null {
  const raw = localStorage.getItem(STORAGE_KEY);
  if (!raw) {
    return null;
  }
  const parsed = Number(raw);
  return Number.isFinite(parsed) ? parsed : null;
}

export function setStoredCreatedBy(userId: number): void {
  localStorage.setItem(STORAGE_KEY, String(userId));
}
