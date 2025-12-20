import { useState, useEffect } from "react";
import { useProducts } from '../../contexts/ProductsContext'

import Product from '../../models/Product'

import '../../assets/styles/ProductForm.css'

function ProductForm( {mode = 'create', product = new Product(), formPhoto, setFormPhoto, onClose })
{
    const {createProduct, updateProduct} = useProducts(); 
    const [formData, setFormData] = useState(product); // current product(new or editable)
    const [errors, setErrors] = useState({}); // param info errors on window
    const [isSubmitting, setIsSubmitting] = useState(false); // param for activity save button
    const [originalProduct, setOriginalProduct] = useState(mode === 'edit' ? product : null);

    function handleChange(e)
    {
        const {name, value, type, checked, files} = e.target;

        if (type === 'file' && files && files[0]) {
            setFormData(prev => ({ ...prev, photo: files[0] }));
            setFormPhoto(URL.createObjectURL(files[0]));
        } 
        else if (type === 'checkbox') {
            setFormData(prev => ({ ...prev, [name]: checked }));
        } 
        else {
            const safeValue = value ?? '';
            setFormData(prev => ({ ...prev, [name]: safeValue }));
        }

        if (errors[name]) {
            setErrors(prev => ({ ...prev, [name]: '' }));
        }
    }

    function validateForm() {
        const newErrors = {};

        if (!formData.name.trim()) {
            newErrors.name = 'Название товара обязательно';
        }

        if (!formData.price || formData.price <= 0) {
            newErrors.price = 'Цена должна быть больше 0';
        }

        if (!formData.quantity || formData.quantity < 0) {
            newErrors.quantity = 'Количество не может быть отрицательным';
        }

        if (!formData.category) {
            newErrors.category = 'Выберите категорию';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    function getChangedFields(original, current) {
        if (!original) return current; // if creating mode - return all

        const changes = {};

        if (current.name !== original.name) changes.name = current.name;
        if (current.price !== original.price) changes.price = current.price;
        if (current.quantity !== original.quantity) changes.quantity = current.quantity;
        if (current.category !== original.category) changes.category = current.category;
        if (current.description !== original.description) changes.description = current.description;
        if (current.isActive !== original.isActive) changes.isActive = current.isActive;

        return changes;
    }

    async function handleSubmit(e) {
        e.preventDefault();

        if (!validateForm()) return;

        setIsSubmitting(true);

        try {
            if (mode === 'create') {
                await createProduct(formData);
            } 
            else {
                // get only editing params
                const changedFields = getChangedFields(originalProduct, formData);
                
                // if edit nothing - nothing change
                if (Object.keys(changedFields).length === 0 && !formData.photo) {
                    onClose();
                    return;
                }

                await updateProduct({
                    id: formData.id,
                    ...changedFields,
                    photo: formData.photo
                });
            }
            
            setFormPhoto(null);
            onClose();
        } catch (error) {
            console.error('Ошибка сохранения товара:', error);
            setErrors({ submit: 'Ошибка при сохранении товара' });
        } finally {
            setIsSubmitting(false);
        }
    };

    function handleCancel() {
        if (window.confirm('Отменить изменения?')) {
            setFormPhoto(null);
            onClose();
        }
    };
    
    return (
        <div className="product-form-overlay">
            <div className="product-form-container">
                <div className="form-header">
                    <h2>{mode === 'create' ? 'Создание товара' : 'Редактирование товара'}</h2>

                    <button className="close-btn"
                            onClick={handleCancel}
                            type="button">
                        × {/*TODO^ change on icon x*/}
                    </button>
                </div>

                <form onSubmit={handleSubmit} className="product-form">
                    <div className="form-section">
                        <h3>Основная информация</h3>

                        <div className="form-group">
                            <label>Изображение товара</label>
                            
                            <div className="photo-upload-container">
                                <input type="file"
                                    id="photo"
                                    name="photo"
                                    onChange={handleChange}
                                    accept="image/*"
                                    className="photo-input" />
                                
                                <label htmlFor="photo" className={"photo-upload-area " + (errors.photo ? 'photo-error' : '')}>
                                    {formData.photo ? (
                                        <>
                                            <img 
                                                src={formPhoto} 
                                                alt="Превью" 
                                                className="photo-preview"
                                            />
                                            <div className="photo-change-text">
                                                Нажмите для изменения фото
                                            </div>
                                        </>
                                    ) : (
                                        <>
                                            <div className="photo-upload-icon">📷</div>
                                            <div className="photo-upload-text">
                                                Нажмите чтобы добавить фото
                                                <br />
                                                <span style={{fontSize: '10px', opacity: 0.7}}>
                                                    JPG, PNG до 2MB
                                                </span>
                                            </div>
                                        </>
                                    )}
                                </label>
                            </div>
                        </div>
                        
                        <div className="form-group">
                            <label htmlFor="name">Название товара *</label>

                            <input type="text"
                                   id="name"
                                   name="name"
                                   value={formData.name}
                                   onChange={handleChange}
                                   placeholder="Введите название товара"
                                   className={errors.name ? 'error' : ''} />

                            {errors.name && <span className="error-text">{errors.name}</span>}
                        </div>

                        <div className="form-row">
                            <div className="form-group">
                                <label htmlFor="price">Цена (₽) *</label>

                                <input type="number"
                                       id="price"
                                       name="price"
                                       value={formData.price}
                                       onChange={handleChange}
                                       placeholder="0"
                                       min="0"
                                       className={errors.price ? 'error' : ''} />

                                {errors.price && <span className="error-text">{errors.price}</span>}
                            </div>

                            <div className="form-group">
                                <label htmlFor="quantity">Количество *</label>

                                <input type="number"
                                       id="quantity"
                                       name="quantity"
                                       value={formData.quantity}
                                       onChange={handleChange}
                                       placeholder="0"
                                       min="0"
                                       className={errors.quantity ? 'error' : ''} />

                                {errors.quantity && <span className="error-text">{errors.quantity}</span>}
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <div className="form-group">
                            <label htmlFor="category">Категория *</label>

                            <select id="category"
                                    name="category"
                                    value={formData.category}
                                    onChange={handleChange}
                                    className={errors.category ? 'error' : ''}>
                                <option value="">Выберите категорию</option>
                                <option value="Монстера">Монстера</option>
                                <option value="Филодендрон">Филодендрон</option>
                                <option value="Сциндапсус">Сциндапсус</option>
                                <option value="Сингониум">Сингониум</option>
                                <option value="Другое">Другое</option>
                            </select>

                            {errors.category && <span className="error-text">{errors.category}</span>}
                        </div>

                        <div className="form-group">
                            <label htmlFor="description">Описание товара</label>

                            <textarea id="description"
                                      name="description"
                                      value={formData.description}
                                      onChange={handleChange}
                                      placeholder="Опишите товар..."
                                      rows="4" />
                        </div>
                    </div>

                    <div className="form-section">
                        <div className="form-group checkbox-group">
                            <label>
                                <input type="checkbox"
                                       name="isActive"
                                       checked={formData.isActive}
                                       onChange={handleChange} />
                                <span className="checkmark"></span>
                                Товар активен
                            </label>
                        </div>
                    </div>

                    {/* Ошибки формы */}
                    {errors.submit && (
                        <div className="form-error">
                            {errors.submit}
                        </div>
                    )}

                    {/* Кнопки действий */}
                    <div className="form-actions">
                        <button
                            type="button"
                            onClick={handleCancel}
                            className="cancel-btn"
                            disabled={isSubmitting}
                        >
                            Отмена
                        </button>
                        <button
                            type="submit"
                            className="submit-btn"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? 'Сохранение...' : (
                                mode === 'create' ? 'Создать товар' : 'Сохранить изменения'
                            )}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default ProductForm;