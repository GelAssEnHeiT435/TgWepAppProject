import { useState, useEffect } from "react";
import { useProducts } from "../../contexts/ProductsContext";

import AdminProductCard from "./AdminProductCard";
import { PlusIcon } from '../common/PlusIcon'

import '../../assets/styles/ProductsManagement.css'

function ProductsManagement()
{
    const { products } = useProducts();

    return(
        <div className="product-list-container">
        {
            products.map(product => { 
                return <AdminProductCard product={product} /> 
            })
        }

            <button className="add-product-fab"
                    title="Добавить товар">
                <PlusIcon />
            </button>
        </div>
    )
}

export default ProductsManagement;