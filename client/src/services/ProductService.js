import Product from '../models/Product'

const API_URL = 'http://localhost:52846/api'

class ProductService
{
    async getAllProducts()
    {
        try {
            const response = await fetch(`${API_URL}/products/all`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            if(!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            const data = await response.json();
            return data.map(productJson => Product.fromJson(productJson));
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось загрузить товары');
        }
    }

    async createProduct(product)
    {
        const formData = new FormData();
        
        formData.append('Name', product.name);
        formData.append('Price', product.price);
        formData.append('Quantity', product.quantity);
        formData.append('Category', product.category);
        formData.append('Description', product.description);
        formData.append('IsActive', product.isActive ? 'true' : 'false');
    
        if (product.photo) {
            formData.append('Image', product.photo); 
        }

        try {
            const response = await fetch(`${API_URL}/products/create`, {
                method: 'POST',
                body: formData
            });

            if(!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            return await response.json();
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось создать товар');
        }
    }

    async updateProduct(product)
    {
        const formData = new FormData();
        
        formData.append("Id", product.id)
        formData.append('Name', product.name);
        formData.append('Price', product.price);
        formData.append('Quantity', product.quantity);
        formData.append('Category', product.category);
        formData.append('Description', product.description);
        formData.append('IsActive', product.isActive ? 'true' : 'false');
    
        if (product.photo instanceof File) {
            formData.append('Image', product.photo); 
        }

        try {
            const response = await fetch(`${API_URL}/products/update`, {
                method: 'PUT',
                body: formData
            });

            if(!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            if (response) return await response.json();
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось обновить товар');
        }
    }

    async deleteProduct(productId)
    {
        try {
            const response = await fetch(`${API_URL}/products/delete?Id=${productId}`, {
                method: 'DELETE'
            });

            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось удалить товар');
        }
    }
}

export default new ProductService();