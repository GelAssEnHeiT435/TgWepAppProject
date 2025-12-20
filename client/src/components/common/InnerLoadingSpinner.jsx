import '../../assets/styles/InnerLoadingSpinner.css'

function InnerLoadingSpinner({ text = '', spinnerSize = 50, textSize = 16})
{
    return (
        <div className="inner-loading-spinner-container">
            <div className="loading">
                <div className="inner-loading-spinner" 
                     style={{ width: spinnerSize, height: spinnerSize }} >
                </div>
            </div>
            <p style={{fontSize: textSize}}>{text}</p>
        </div>
    )
}

export default InnerLoadingSpinner;