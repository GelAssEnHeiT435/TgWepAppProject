import { useState, useEffect } from "react";
import "../../assets/styles/ProductCard.css"

function ProductCard({photo = "", name = "", price = 0})
{
    const [count, setCount] = useState(0);

    function navigateToInfo()
    {
        console.log("navigated to info page");
    }

    function handleIncrement(event){
        event.stopPropagation();
        setCount(count + 1)
    }

    function handleDecrement(event){
        event.stopPropagation();
        setCount(count - 1)
    }

    useEffect( () => {

    }, [count])

    return (
        <div className="product-button-container">
            <button onClick={ () => navigateToInfo() } className="product-button">
                <img src={photo} className="product-image"/> 
                <p className="product-name">{name}</p>
                <p className="product-price">{price}</p>

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