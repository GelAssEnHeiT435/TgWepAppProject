import { useProducts } from "../../contexts/ProductsContext";

import logo from "../../assets/icons/flower1.jpg";

import '../../assets/styles/AdminProductCard.css'

function AdminProductCard({ product })
{
    const { createProduct, updateProduct, deleteProduct, changeActiveProduct} = useProducts();

    const handleDelete = () => {
        if (window.confirm(`Удалить товар "${product.name}"?`)) {
            deleteProduct(product.id);
        }
    };

    return (
        <div className="product-container">
            {/* image container */}
            <div className="admin-image-container">
                <img src={product.photo || logo} 
                     alt={product.name}
                     className="admin-product-image"/>
            </div>

            {/* info container */}
            <div className="admin-info-container">
                <h3 className="admin-product-name">{product.name}</h3>
                <p className="admin-product-price">{product.price.toLocaleString()} ₽</p>

                {product.description && (
                    <p className="admin-product-description" title={product.description}>{product.description}</p>
                )}

                <div className="admin-product-meta">
                    <span className="admin-meta-item">📦 {product.quantity} шт.</span>
                    <span className="admin-meta-item">🏷️ {product.category}</span>
                    <span className="admin-meta-item">📅 {new Date(product.createdAt).toLocaleDateString()}</span>
                </div>
            </div>

            {/* buttons container */}
            <div className="admin-actions-container">
                <div className="admin-product-status">
                    <div className={`status-dot ${product.isActive ? 'active' : 'inactive'}`}></div>
                    <span className={product.isActive ? 'status-active' : 'status-inactive'}>
                        {product.isActive ? 'Активен' : 'Неактивен'}
                    </span>
                </div>
                <button 
                    className="admin-action-btn edit-btn"
                    title="Редактировать"
                    onClick={() => {/* будет позже */}}
                >
                    ✏️
                </button>
                
                <button 
                    className="admin-action-btn status-btn"
                    onClick={() => changeActiveProduct(product.id)}
                    title={product.isActive ? 'Деактивировать' : 'Активировать'}
                >
                    {product.isActive ? '👁️' : '👁️‍🗨️'}
                </button>
                
                <button 
                    className="admin-action-btn delete-btn"
                    onClick={handleDelete}
                    title="Удалить"
                >
                    🗑️
                </button>
            </div>
        </div>
    )
}

export default AdminProductCard;