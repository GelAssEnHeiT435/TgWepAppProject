import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import TelegramProtectedRoute from './routes/TelegramProtectedRoute';
import LoadingSpinner from './components/common/LoadingSpinner';
import DevelopmentAuth from './components/layout/DevelopmentAuth';
import { TelegramAuthProvider, useTelegramAuth } from './contexts/TelegramAuthContext';
import './assets/styles/App.css';

function AppContent()
{
  const {user, loading} = useTelegramAuth()

  if (loading) return <LoadingSpinner />

  return (
    <div className='App'>
      {!window.Telegram?.WebApp && <DevelopmentAuth />}

      <Routes>
        <Route path="/" element={
          <TelegramProtectedRoute>
            
          </TelegramProtectedRoute>
        } />
      </Routes>
    </div>
  )
}

function App() {
  return (
    <TelegramAuthProvider>
      <Router>
        <AppContent />
      </Router>
    </TelegramAuthProvider>
  );
}

export default App;
