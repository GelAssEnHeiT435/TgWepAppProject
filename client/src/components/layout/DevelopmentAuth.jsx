import React, { useState } from 'react';
import { useTelegramAuth } from '../../contexts/TelegramAuthContext';
//import './DevelopmentAuth.css';

const DevelopmentAuth = () => {
  const [telegramId, setTelegramId] = useState('');
  const { user } = useTelegramAuth();

  const handleLogin = (id, firstName, username) => {
    // В реальном приложении это будет автоматически из Telegram WebApp
    console.log(`Development login with ID: ${id}`);
  };

  const predefinedUsers = [
    { id: 123456789, name: 'Admin User', username: 'admin' },
    { id: 987654321, name: 'Second Admin', username: 'admin2' },
    { id: 111111111, name: 'Regular User', username: 'user1' },
    { id: 222222222, name: 'Another User', username: 'user2' }
  ];

  return (
    <div className="development-auth">
      <h3>🔧 Development Mode</h3>
      <p>Telegram WebApp не обнаружен. Выберите тестового пользователя:</p>
      
      <div className="test-users">
        {predefinedUsers.map(testUser => (
          <button
            key={testUser.id}
            className={`test-user-btn ${user?.id === testUser.id ? 'active' : ''}`}
            onClick={() => handleLogin(testUser.id, testUser.name, testUser.username)}
            disabled
          >
            {testUser.name} (ID: {testUser.id})
            {[123456789, 987654321].includes(testUser.id) && ' 👑'}
          </button>
        ))}
      </div>
      
      <div className="current-user">
        <strong>Текущий пользователь:</strong>
        {user ? (
          <div>
            {user.firstName} (ID: {user.id}) - {user.role}
            {user.isAdmin && ' 👑'}
          </div>
        ) : (
          <div>Не аутентифицирован</div>
        )}
      </div>
    </div>
  );
};

export default DevelopmentAuth;