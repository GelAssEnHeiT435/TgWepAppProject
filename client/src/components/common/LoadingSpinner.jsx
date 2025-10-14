import '../../assets/styles/LoadingSpinner.css'

function LoadingSpinner()
{
    return (
        <div className="loading-spinner-container">
            <div className="telegram-loading">
                <div className="loading-spinner">
                    <p>Загрузка Telegram Mini App...</p>
                </div>
            </div>
        </div>
    )
}

export default LoadingSpinner;