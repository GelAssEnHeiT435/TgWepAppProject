import React, {createContext, useContext, useState, useEffect } from "react";

const CatalogContext = createContext();

export function useCatalog()
{
    const context = useContext(CatalogContext)
    if (!context) throw new Error('CatalogContext must be used within CatalogProvider');
    return context
}

export function CatalogProvider({children})
{
    const [basketItems, setBasketItems] = useState({}); // Object - productIds : Quantity

    function addToBasket(productId)
    {
        setBasketItems(prev => ({ ...prev, 
            [productId]: (prev[productId] || 0) + 1
        }));
    };

    function removeFromBasket(productId)
    {
        setBasketItems(prev => {
            const newItems = { ...prev }

            if (newItems[productId] > 1) newItems[productId] -= 1;
            else delete newItems[productId];

            return newItems;
        });
    };

    function getTotalItems()
    {
        return Object.values(basketItems).reduce((total, quantity) => total + quantity, 0);
    }

    function getItemQuantity(productId)
    {
        return basketItems[productId] || 0;
    }

    const value = {
        basketItems,
        addToBasket,
        removeFromBasket,
        getTotalItems,
        getItemQuantity
    };

    return (
        <CatalogContext.Provider value={value}>
            {children}
        </CatalogContext.Provider>
    )
}