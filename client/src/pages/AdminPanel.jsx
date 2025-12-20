import { useState } from "react";

import AdminSidebar from '../components/layout/AdminTabs'
import Dashboard from '../components/layout/Dashboard'
import ProductsManagement from '../components/layout/ProductsManagement'
import OrdersManagement from '../components/layout/OrdersManagement'
import GiftsManagement from '../components/layout/GiftsManagement'

import '../assets/styles/AdminPanel.css'

function AdminPanel()
{
    const [activeSection, setActiveSection] = useState('products');

    const adminSections = [
        { id: 'dashboard', icon: '📊', label: 'Дашборд' },
        { id: 'products', icon: '🌿', label: 'Товары' },
        { id: 'orders', icon: '📦', label: 'Заказы' },
        { id: 'giveaways', icon: '🎯', label: 'Розыгрыши' },
    ];

    const renderSection = () => {
        switch (activeSection) {
            case 'dashboard': return <Dashboard />;
            case 'products': return <ProductsManagement />;
            case 'orders': return <OrdersManagement />;
            case 'giveaways': return <GiftsManagement />;
            default: return <Dashboard />;
        }
    };

    return (
        <div className="admin-panel">
            <AdminSidebar sections={adminSections}
                          activeSection={activeSection}
                          onSectionChange={setActiveSection}/>

            <div className="admin-content">
                {renderSection(activeSection)}
            </div>
        </div>
    );
}

export default AdminPanel;