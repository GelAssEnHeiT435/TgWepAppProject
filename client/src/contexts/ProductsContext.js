import { createContext, useContext, useState, useEffect } from "react";

import ProductService from '../services/ProductService'
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
        loadProducts()
    }, [])

    async function loadProducts() {
        try {
            const productsData = await ProductService.getAllProducts();
            console.log(productsData);
            setProducts(productsData);
        } catch (err) {
            console.error('Error loading products:', err);
        } 
    };

    async function createProduct(product)
    {
        try {
            const dataJson = await ProductService.createProduct(product);

            const newProduct = product instanceof Product 
                ? { ...product, id: dataJson.id, photo: dataJson.image }
                : { 
                    ...product,           
                    id: dataJson.id,      
                    photo: dataJson.image
                  };

            setProducts(prev => [newProduct, ...prev]);
        }
        catch (err) {
            console.error('Error created product:', err);
        }
    }

    async function updateProduct(Uproduct)
    {
        try {
            const dataJson = await ProductService.updateProduct(Uproduct);

            setProducts(prevProducts => 
                prevProducts.map(product => {
                    if(product.id === Uproduct.id) {
                        const newPhoto = dataJson?.image ?? product.photo;
                        return { ...Uproduct, 
                            photo: dataJson?.image ?? product.photo, 
                            updatedAt: dataJson?.update ?? product.updatedAt}
                    }
                    return product;
                })
            );
        }
        catch (err) {
            console.error('Error updating product: ', err)
        }
    }

    async function deleteProduct(productId)
    {
        await ProductService.deleteProduct(productId);

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