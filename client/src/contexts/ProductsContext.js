import { createContext, useContext, useState, useEffect } from "react";

import Product from '../models/Product'

const ProductsContext = createContext();

export function useProducts()
{
    const context = useContext(ProductsContext);
    if (!context){
        throw new Error('useProducts must be used within a ProductsProvider');
    }
    return context;
}

export function ProductsProvider({children})
{
    const [products, setProducts] = useState([]);

    useEffect(() => {
        setProducts([new Product(1), new Product(2), new Product(3)])
    }, [])

    function createProduct()
    {

    }

    function updateProduct(Uproduct)
    {
        setProducts(prevProducts => 
            prevProducts.map(product => {
                if(product.id === Uproduct.id) return { ...Uproduct, updatedAt: new Date() }
            })
        );
    }

    function deleteProduct(productId)
    {
        setProducts(prevProducts => 
            prevProducts.filter(product => product.id !== productId)
        );
    }

    function changeActiveProduct(productId)
    {
        setProducts(prevProducts => 
            prevProducts.map(product => 
                product.id === productId 
                    ? { ...product, isActive: !product.isActive, updatedAt: new Date() }
                    : product
            )
        );
    }

    const value = {
        products,
        createProduct,
        updateProduct,
        deleteProduct,
        changeActiveProduct,
    }

    return (
        <ProductsContext.Provider value={value}>
            {children}
        </ProductsContext.Provider>
    );
}