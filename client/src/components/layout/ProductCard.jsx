import { useState, useEffect } from "react";
import { useCatalog } from "../../contexts/CatalogContext";
import "../../assets/styles/ProductCard.css"

function ProductCard({id = null, photo = "", name = "", price = 0, quantity = 0})
{
    const {addToBasket, removeFromBasket, getItemQuantity} = useCatalog();
    const count = getItemQuantity(id);

    function navigateToInfo()
    {
        console.log("navigated to info page");
    }

    function handleIncrement(event){
        event.stopPropagation();
        if (count >= quantity) return;
        addToBasket(id);
    }

    function handleDecrement(event){
        event.stopPropagation();
        removeFromBasket(id);
    }

    useEffect( () => {

    }, [count])

    return (
        <div className="product-button-container">
            <button onClick={ () => navigateToInfo() } className="product-button">
                <img src={photo} className="product-image"/> 
                <div className="price-limit-container">
                    <p className="product-price">{(price ?? 0).toLocaleString()} ₽</p>
                    <p className="limit-info">В наличии: {quantity} шт.</p>
                </div>
                <p className="product-name">{name}</p>

                <div className="counter-button-container">
                {
                    count === 0 ? 
                    (
                        <button onClick={handleIncrement}
                                className="start-product-button"> Добавить </button>
                    ) : (
                        <>
                            <button onClick={handleDecrement} className="counter-button"> — </button>
                            <span className="counter-display"> {count} </span>
                            <button onClick={handleIncrement} className="counter-button"> + </button>
                        </>
                    )
                }
                </div>
            </button>
        </div>
    );
}

export default ProductCard;