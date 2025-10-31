import '../../assets/styles/AdminTabs.css'

function AdminTabs({ sections, activeSection, onSectionChange })
{
    return(
        <div className="admin-tabs">
        {
            sections.map(section => (
                <button className={`tab-btn ${activeSection === section.id ? 'active' : ''}`}
                        onClick={() => onSectionChange(section.id)}>
                    <span className="btn-icon">{section.icon}</span>
                    <span className="btn-label">{section.label}</span>
                </button>
            ))
        }
      </div>
    )
}

export default AdminTabs;