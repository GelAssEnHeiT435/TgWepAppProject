import { useState, useEffect } from "react";
import "../../assets/styles/ProductCard.css"

function ProductCard({photo = "", name = "", price = 0})
{
    const [count, setCount] = useState(0);

    function navigateToInfo()
    {
        console.log("navigated to info page");
    }

    useEffect( () => {

    }, [count])

    return (
        <div className="product-button-container">
            <button onClick={ () => navigateToInfo() }
                    className="product-button">
                <img src={photo} className="product-image"/> 
                <p className="product-name">{name}</p>
                <p className="product-price">{price}</p>

                {
                    count === 0 ? 
                    (
                        <button onClick={ () => setCount(count + 1) }
                                className=""> Добавить </button>
                    ) : (
                        <div className="counter-button-container">
                            <button onClick={ () => setCount(count - 1) } className="counter-button"> - </button>
                            <span className="counter-button"> {count} </span>
                            <button onClick={ () => setCount(count + 1) } className="counter-button"> + </button>
                        </div>
                    )
                }
            </button>
        </div>
    );
}

export default ProductCard;