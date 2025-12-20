import Product from '../models/Product'
import apiClient from './ApiClient';

const API_URL = 'http://localhost:52846/api'

class ProductService
{
    async getAllProducts()
    {
        try {
            const response = await apiClient.get(`/products`)
            return response.data.map(productJson => Product.fromJson(productJson));
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
            const response = await apiClient.postForm(`/products`, formData)
            return await response.data;
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось создать товар');
        }
    }

    async updateProduct(product)
    {
        const formData = new FormData();
  
        if (product.name !== undefined) formData.append('Name', product.name);
        if (product.price !== undefined) formData.append('Price', product.price);
        if (product.quantity !== undefined) formData.append('Quantity', product.quantity);
        if (product.category !== undefined) formData.append('Category', product.category);
        if (product.description !== undefined) formData.append('Description', product.description);
        if (product.isActive !== undefined) formData.append('IsActive', product.isActive ? 'true' : 'false');

        if (product.photo instanceof File) {
            formData.append('Image', product.photo);
        }

        try {
            const response = await apiClient.patchForm(`/products?Id=${product.id}`, formData)
            if (response) return await response.data;
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось обновить товар');
        }
    }

    async deleteProduct(productId)
    {
        try {
            const response = await apiClient.delete(`/products?Id=${productId}`)
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось удалить товар');
        }
    }
}

export default new ProductService();