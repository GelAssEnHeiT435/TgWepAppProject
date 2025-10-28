import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { TelegramAuthProvider, useTelegramAuth } from './contexts/TelegramAuthContext';
import { CatalogProvider } from './contexts/CatalogContext';

import TelegramProtectedRoute from './routes/TelegramProtectedRoute';
import LoadingSpinner from './components/common/LoadingSpinner';
import Catalog from "./pages/Catalog"
import Header from "./components/layout/Header"

import './assets/styles/App.css';

function App() {
  return (
    <TelegramAuthProvider>
      <CatalogProvider>
        <Router>
          <AppContent />
        </Router>
      </CatalogProvider>
    </TelegramAuthProvider>
  );
}

function AppContent()
{
  const {user, loading} = useTelegramAuth()

  if (loading) return <LoadingSpinner />

  return (
    <div className='App'>
      <Header />
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

export default App;
