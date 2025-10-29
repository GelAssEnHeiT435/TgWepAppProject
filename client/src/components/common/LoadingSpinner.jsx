import '../../assets/styles/LoadingSpinner.css'

function LoadingSpinner()
{
    return (
        <div className="loading-spinner-container">
            <div className="telegram-loading">
                <div className="loading-spinner">
                </div>
            </div>
            <p>Загрузка Telegram Mini App...</p>
        </div>
    )
}

export default LoadingSpinner;