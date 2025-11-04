class Product {
    constructor(id = 0,
                name = 'вечнозеленая лиана из семейства ароидных с крупными глянцевыми листьями, которые с возрастом становятся резными, прорезными или перфорированными. В естественной среде она может достигать огромных размеров, обвивая деревья с помощью воздушных корней, которые в домашних условиях служат для опоры. Монстера быстро растет, отличается неприхотливостью и ценится за декоративный внешний вид, а также способностью очищать воздух',
                price = 1000,
                quantity = 10,
                category = 'монстера',
                description = 'вечнозеленая лиана из семейства ароидных с крупными глянцевыми листьями, которые с возрастом становятся резными, прорезными или перфорированными. В естественной среде она может достигать огромных размеров, обвивая деревья с помощью воздушных корней, которые в домашних условиях служат для опоры. Монстера быстро растет, отличается неприхотливостью и ценится за декоративный внешний вид, а также способностью очищать воздух',
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
            json.id || 0,
            json.name || '',
            json.price || 0,
            json.quantity || 0,
            json.category || '',
            json.description || '',
            json.photo || null,
            json.isActive || false,
            json.createdAt || new Date(),
            json.updatedAt || new Date()
        );
    }

    isValid() {
        return this.id > 0 && 
               this.name && this.name.length > 0 ;
    }

    static toJson() {
        return {
            id: this.id,
            name: this.name,
            price: this.price,
            quantity: this.quantity,
            category: this.category,
            description: this.description,
            photos: this.photo,
            isActive: this.isActive,
            createdAt: this.createdAt,
            updatedAt: this.updatedAt
        };
    }
}

export default Product;