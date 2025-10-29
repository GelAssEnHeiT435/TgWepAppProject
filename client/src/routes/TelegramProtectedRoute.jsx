import LoadingSpinner from '../components/common/LoadingSpinner';
import { useTelegramAuth } from '../contexts/TelegramAuthContext'

import './assets/styles/LoadingSpinner.css';

function TelegramProtectedRoute({children, adminOnly = false})
{
    const { user, loading, isAuthenticated, isAdmin} = useTelegramAuth();

    if (loading) return ( <LoadingSpinner />)

    if (!isAuthenticated) return (
        <div className='auth-error'>
            <h2>Требуется авторизация</h2>
            <p>Пожалуйста, откройте это приложение через Telegram бота</p>
        </div>
    );
    
    if (adminOnly && !isAdmin) return (
        <div className='auth-error'>
            <h2>Доступ запрещен</h2>
            <p>У вас недостаточно прав для просмотра этой страницы</p>
        </div>
    )

    return children;
}

export default TelegramProtectedRoute;