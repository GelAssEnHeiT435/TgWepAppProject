import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import TelegramProtectedRoute from './routes/TelegramProtectedRoute';
import LoadingSpinner from './components/common/LoadingSpinner';
import Catalog from "./pages/Catalog"
import { TelegramAuthProvider, useTelegramAuth } from './contexts/TelegramAuthContext';
import './assets/styles/App.css';

function AppContent()
{
  const {user, loading} = useTelegramAuth()

  if (loading) return <LoadingSpinner />

  return (
    <div className='App'>
      <Catalog />

      {/* <Routes>
        <Route path="/" element={
          <TelegramProtectedRoute>
            
          </TelegramProtectedRoute>
        } />
      </Routes> */}
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
