import Product from '../models/Product'
import apiClient from './ApiClient';

class CatalogService
{
    async getCatalog()
    {
        try {
            const response = await apiClient.get(`/catalog`);
            return response.data.map(productJson => Product.fromJson(productJson));
        }
        catch (error) {
            console.error('Error fetching products:', error);
            throw new Error('Не удалось загрузить товары');
        }
    }
}

export default new CatalogService();