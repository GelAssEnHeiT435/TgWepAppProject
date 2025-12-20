class Product {
    constructor(id = 0,
                name = '',
                price = null,
                quantity = null,
                category = '',
                description = '',
                photo = null,
                isActive = true,
                createdAt = new Date(),
                updatedAt = new Date()) 
    {
        this.id = id;
        this.name = name;
        this.price = price;
        this.quantity = quantity;
        this.category = category;
        this.description = description;
        this.photo = photo;
        this.isActive = isActive;
        this.createdAt = createdAt;
        this.updatedAt = updatedAt;
    }

    static fromJson(json) {
        return new Product(
            json.id,
            json.name,
            json.price,
            json.quantity,
            json.category,
            json.description,
            json.photo,
            json.hasOwnProperty('is_active') ? Boolean(json.is_active) : false,
            new Date(json.created_at),
            new Date(json.updated_at)
        );
    }

    toJson() {
        return {
            id: this.id,
            name: this.name,
            price: this.price,
            quantity: this.quantity,
            category: this.category,
            description: this.description,
            photo: this.photo,
            isActive: this.isActive,
            createdAt: this.createdAt,
            updatedAt: this.updatedAt
        };
    }
}

export default Product;