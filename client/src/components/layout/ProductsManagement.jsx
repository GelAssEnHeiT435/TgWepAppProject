import { useState, useEffect } from "react";
import { useProducts } from "../../contexts/ProductsContext";

import AdminProductCard from "./AdminProductCard";
import ProductForm from "../forms/ProductForm";
import Product from '../../models/Product';
import InnerLoadingSpinner from '../common/InnerLoadingSpinner'
import { PlusIcon } from '../common/PlusIcon';

import '../../assets/styles/ProductsManagement.css';

function ProductsManagement()
{
    const { products, createProduct, updateProduct, loading } = useProducts();
    const [isFormOpen, setIsFormOpen] = useState(false);
    const [editingProduct, setEditingProduct] = useState(null);
    const [formPhoto, setFormPhoto] = useState(null); // param for product preview

    function handleCreateProduct(){
        setEditingProduct(new Product()); 
        setIsFormOpen(true);
    };

    function handleEditProduct(product, photo = null) {
        setEditingProduct(product); 
        setFormPhoto(photo)
        setIsFormOpen(true);
    };

    function handleFormClose() {
        setIsFormOpen(false);
        setEditingProduct(null);
    };

    if (loading) return <InnerLoadingSpinner text={'Загрузка данных о продуктах...'} />

    return(
        <div className="product-list-container">
            <button className="add-product-fab"
                    onClick={handleCreateProduct}
                    title="Добавить товар">
                <PlusIcon />
            </button>
        {
            products.map(product => { 
                return <AdminProductCard product={product}
                                         onEdit={handleEditProduct} /> 
            })
        }

        {isFormOpen && (
            <ProductForm product={editingProduct}
                         mode={editingProduct?.id ? 'edit' : 'create'}
                         formPhoto={formPhoto}
                         setFormPhoto={setFormPhoto}
                         onClose={handleFormClose} />
        )}
        </div>
    )
}

export default ProductsManagement;