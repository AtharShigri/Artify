import React, { useState, useEffect } from 'react';
import { Upload, X, Shield } from 'lucide-react';
import { useNavigate, useParams } from 'react-router-dom';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import artworkService from '../../services/artworkService';
import { ART_CATEGORIES } from '../../constants/categories';
import Loader from '../../components/common/Loader';

const BACKEND_URL = import.meta.env.VITE_API_URL
    ? new URL(import.meta.env.VITE_API_URL).origin
    : 'http://localhost:5181';

const UploadArtwork = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const isEditMode = Boolean(id);

    const [dragActive, setDragActive] = useState(false);
    const [file, setFile] = useState(null);
    const [existingImageUrl, setExistingImageUrl] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [isFetching, setIsFetching] = useState(isEditMode);

    const [formData, setFormData] = useState({
        title: '',
        price: '',
        description: '',
        category: '',
        width: '',
        height: '',
        year: '',
        isAvailable: true,
        applyWatermark: false,
        registerFingerprint: false,
        copyrightText: '',
    });

    useEffect(() => {
        if (!isEditMode) return;

        const loadArtwork = async () => {
            try {
                const data = await artworkService.getById(id);
                if (!data) {
                    alert('Artwork not found.');
                    navigate('/dashboard/artist/artworks');
                    return;
                }
                setFormData({
                    title: data.title || '',
                    price: data.price || '',
                    description: data.description || '',
                    category: data.categoryEntity?.name || '',
                    width: '',
                    height: '',
                    year: '',
                    isAvailable: data.isForSale ?? true,
                    applyWatermark: data.imageUrl?.includes('/watermarked/') || false,
                    registerFingerprint: data.protectionStatus?.isFingerprinted || false,
                    copyrightText: data.protectionStatus?.metadata?.copyrightNotice || '',
                });
                if (data.imageUrl) {
                    setExistingImageUrl(
                        data.imageUrl.startsWith('http')
                            ? data.imageUrl
                            : `${BACKEND_URL}${data.imageUrl}`
                    );
                }
            } catch (error) {
                console.error('Failed to load artwork', error);
                alert('Failed to load artwork.');
                navigate('/dashboard/artist/artworks');
            } finally {
                setIsFetching(false);
            }
        };

        loadArtwork();
    }, [id, isEditMode, navigate]);

    const handleDrag = (e) => {
        e.preventDefault();
        e.stopPropagation();
        if (e.type === 'dragenter' || e.type === 'dragover') setDragActive(true);
        else if (e.type === 'dragleave') setDragActive(false);
    };

    const handleDrop = (e) => {
        e.preventDefault();
        e.stopPropagation();
        setDragActive(false);
        if (e.dataTransfer.files?.[0]) setFile(e.dataTransfer.files[0]);
    };

    const handleFileChange = (e) => {
        if (e.target.files?.[0]) setFile(e.target.files[0]);
    };

    const handleInputChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({ ...prev, [name]: type === 'checkbox' ? checked : value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!isEditMode && !file) {
            alert('Please select a file first.');
            return;
        }

        setIsLoading(true);
        try {
            const data = new FormData();
            data.append('Title', formData.title);
            data.append('Description', formData.description);
            data.append('Price', formData.price);
            data.append('IsAvailable', formData.isAvailable);

            const metadataObj = { width: formData.width, height: formData.height, year: formData.year };
            data.append('Metadata', JSON.stringify(metadataObj));

            if (isEditMode) {
                if (file) data.append('File', file); // only send if user picked a new one
                data.append('ApplyWatermark', formData.applyWatermark);
                data.append('RegisterFingerprint', formData.registerFingerprint);
                data.append('CopyrightText', formData.copyrightText);
                
                await artworkService.update(id, data);
                alert('Artwork updated successfully!');
            } else {
                data.append('Category', formData.category);
                data.append('File', file);
                data.append('ApplyWatermark', formData.applyWatermark);
                data.append('RegisterFingerprint', formData.registerFingerprint);
                data.append('CopyrightText', formData.copyrightText);

                await artworkService.create(data);
                alert('Artwork uploaded successfully!');
            }

            navigate('/dashboard/artist/artworks');
        } catch (error) {
            console.error('Submit failed:', error.response?.data);
            const msg = error.response?.data?.errors
                ? 'Validation failed. Please check your inputs.'
                : `Failed to ${isEditMode ? 'update' : 'upload'} artwork. Please try again.`;
            alert(msg);
        } finally {
            setIsLoading(false);
        }
    };

    if (isFetching) return <Loader />;

    return (
        <div className="max-w-3xl mx-auto">
            <div className="mb-8">
                <h1 className="text-2xl font-heading font-bold text-primary">
                    {isEditMode ? 'Edit Artwork' : 'Upload New Artwork'}
                </h1>
                <p className="text-textSecondary">
                    {isEditMode ? 'Update your artwork details' : 'Share your masterpiece with the world'}
                </p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-8 bg-white p-8 rounded-2xl shadow-sm border border-border">

                {/* Image upload / replace */}
                <div>
                    <label className="block text-sm font-medium text-textSecondary mb-2">
                        {isEditMode ? 'Replace Image (optional)' : 'Artwork Image'}
                    </label>

                    {/* Show existing image thumbnail in edit mode */}
                    {isEditMode && existingImageUrl && !file && (
                        <div className="mb-3 flex items-center gap-3">
                            <img
                                src={existingImageUrl}
                                alt="Current artwork"
                                className="w-20 h-20 rounded-lg object-cover border border-border"
                            />
                            <span className="text-sm text-textSecondary">Current image — pick a new file below to replace it</span>
                        </div>
                    )}

                    <div
                        className={`relative border-2 border-dashed rounded-xl p-8 text-center transition-colors ${dragActive ? 'border-primary bg-primary/5' : 'border-gray-300 hover:border-primary'}`}
                        onDragEnter={handleDrag}
                        onDragLeave={handleDrag}
                        onDragOver={handleDrag}
                        onDrop={handleDrop}
                    >
                        <input
                            type="file"
                            className="absolute inset-0 w-full h-full opacity-0 cursor-pointer"
                            onChange={handleFileChange}
                            accept="image/*"
                        />

                        {file ? (
                            <div className="relative z-10">
                                <div className="flex items-center justify-center gap-2 mb-2">
                                    <span className="font-medium text-primary">{file.name}</span>
                                    <button
                                        type="button"
                                        onClick={(e) => { e.preventDefault(); setFile(null); }}
                                        className="p-1 hover:bg-gray-200 rounded-full"
                                    >
                                        <X className="w-4 h-4 text-gray-500" />
                                    </button>
                                </div>
                                <p className="text-xs text-textSecondary">{(file.size / 1024 / 1024).toFixed(2)} MB</p>
                            </div>
                        ) : (
                            <div className="flex flex-col items-center">
                                <div className="w-12 h-12 bg-gray-50 rounded-full flex items-center justify-center mb-4">
                                    <Upload className="w-6 h-6 text-gray-400" />
                                </div>
                                <p className="font-medium text-primary mb-1">Click to upload or drag and drop</p>
                                <p className="text-xs text-textSecondary">SVG, PNG, JPG or GIF (max. 10MB)</p>
                            </div>
                        )}
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <Input
                        name="title"
                        label="Artwork Title"
                        placeholder="e.g. Midnight Dreams"
                        required
                        value={formData.title}
                        onChange={handleInputChange}
                    />
                    <Input
                        name="price"
                        label="Price (PKR)"
                        type="number"
                        placeholder="0.00"
                        required
                        value={formData.price}
                        onChange={handleInputChange}
                    />
                </div>

                <div>
                    <label className="block text-sm font-medium text-textSecondary mb-1.5">Description</label>
                    <textarea
                        name="description"
                        className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary min-h-[120px]"
                        placeholder="Tell the story behind your artwork..."
                        value={formData.description}
                        onChange={handleInputChange}
                    />
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                    <div>
                        <label className="block text-sm font-medium text-textSecondary mb-1.5">Category</label>
                        <select
                            name="category"
                            className="w-full px-4 py-3 rounded-lg border border-border bg-white focus:outline-none focus:ring-2 focus:ring-secondary/20"
                            value={formData.category}
                            onChange={handleInputChange}
                        >
                            <option value="">Select a Category</option>
                            {ART_CATEGORIES.map((cat) => (
                                <option key={cat} value={cat}>{cat}</option>
                            ))}
                        </select>
                    </div>
                    <Input
                        name="width"
                        label="Width (inches)"
                        placeholder="e.g. 24"
                        value={formData.width}
                        onChange={handleInputChange}
                    />
                    <Input
                        name="height"
                        label="Height (inches)"
                        placeholder="e.g. 36"
                        value={formData.height}
                        onChange={handleInputChange}
                    />
                    <Input
                        name="year"
                        label="Year Created"
                        placeholder="e.g. 2024"
                        value={formData.year}
                        onChange={handleInputChange}
                    />
                </div>

                {/* Availability — shown in both modes */}
                <div className="flex items-center gap-3">
                    <input
                        type="checkbox"
                        id="isAvailable"
                        name="isAvailable"
                        checked={formData.isAvailable}
                        onChange={handleInputChange}
                        className="w-4 h-4 accent-secondary"
                    />
                    <label htmlFor="isAvailable" className="text-sm font-medium text-textSecondary">
                        Available for Sale
                    </label>
                </div>

                {/* Protection Suite */}
                <div className="pt-6 border-t border-border">
                    <h3 className="text-lg font-heading font-semibold text-primary mb-4 flex items-center gap-2">
                        <Shield className="w-5 h-5 text-secondary" />
                        Artwork Protection Suite
                    </h3>
                    
                    <div className="space-y-4">
                        <div className="flex items-center gap-3">
                            <input
                                type="checkbox"
                                id="applyWatermark"
                                name="applyWatermark"
                                checked={formData.applyWatermark}
                                onChange={handleInputChange}
                                className="w-4 h-4 accent-secondary"
                            />
                            <label htmlFor="applyWatermark" className="text-sm font-medium text-textSecondary">
                                Apply Visible Watermark (Copyright text tiled across image)
                            </label>
                        </div>

                        <div className="flex items-center gap-3">
                            <input
                                type="checkbox"
                                id="registerFingerprint"
                                name="registerFingerprint"
                                checked={formData.registerFingerprint}
                                onChange={handleInputChange}
                                className="w-4 h-4 accent-secondary"
                            />
                            <label htmlFor="registerFingerprint" className="text-sm font-medium text-textSecondary">
                                Register Digital Fingerprint (SHA-256 + pHash)
                            </label>
                        </div>

                        <div>
                            <Input
                                name="copyrightText"
                                label="Copyright Notice"
                                placeholder="e.g. © 2024 Artist Name. All rights reserved."
                                value={formData.copyrightText}
                                onChange={handleInputChange}
                            />
                            <p className="text-xs text-textSecondary mt-1">
                                This will be embedded in the platform metadata registry.
                            </p>
                        </div>
                    </div>
                </div>

                <div className="flex justify-end gap-3 pt-4">
                    {isEditMode && (
                        <Button
                            type="button"
                            variant="outline"
                            onClick={() => navigate('/dashboard/artist/artworks')}
                        >
                            Cancel
                        </Button>
                    )}
                    <Button
                        type="submit"
                        variant="primary"
                        isLoading={isLoading}
                        disabled={!isEditMode && !file}
                    >
                        {isEditMode ? 'Save Changes' : 'Publish Artwork'}
                    </Button>
                </div>
            </form>
        </div>
    );
};

export default UploadArtwork;
