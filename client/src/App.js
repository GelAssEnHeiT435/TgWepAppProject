import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { TelegramAuthProvider, useTelegramAuth } from './contexts/TelegramAuthContext';
import { CatalogProvider } from './contexts/CatalogContext';

import TelegramProtectedRoute from './routes/TelegramProtectedRoute';
import LoadingSpinner from './components/common/LoadingSpinner';
import Catalog from "./pages/Catalog"
import Gifts from './pages/Gifts';
import Profile from './pages/Profile'
import AdminPanel from './pages/AdminPanel'
import About from './pages/About'
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

      <Routes>
        <Route path="/" element={
          <TelegramProtectedRoute>
            <Catalog />
          </TelegramProtectedRoute>
        }/>

        <Route path="/gifts" element={
          <TelegramProtectedRoute>
            <Gifts />
          </TelegramProtectedRoute>
        }/>

        <Route path="/profile" element={
          <TelegramProtectedRoute>
            <Profile />
          </TelegramProtectedRoute>
        } />

        <Route path="/admin" element={
          <TelegramProtectedRoute adminOnly={true}>
            <AdminPanel />
          </TelegramProtectedRoute>
        } />

        <Route path='/about' element={
          <TelegramProtectedRoute>
            <About />
          </TelegramProtectedRoute>
        } />
      </Routes>
    </div>
  )
}

export default App;
