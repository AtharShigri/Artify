import React, { useState } from 'react';
import { Upload, X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import artworkService from '../../services/artworkService';

const UploadArtwork = () => {
    const navigate = useNavigate();
    const [dragActive, setDragActive] = useState(false);
    const [file, setFile] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    // Form state
    const [formData, setFormData] = useState({
        title: '',
        price: '',
        description: '',
        category: 'Visual Arts',
        width: '',
        height: '',
        year: ''
    });

    const handleDrag = (e) => {
        e.preventDefault();
        e.stopPropagation();
        if (e.type === "dragenter" || e.type === "dragover") {
            setDragActive(true);
        } else if (e.type === "dragleave") {
            setDragActive(false);
        }
    };

    const handleDrop = (e) => {
        e.preventDefault();
        e.stopPropagation();
        setDragActive(false);
        if (e.dataTransfer.files && e.dataTransfer.files[0]) {
            setFile(e.dataTransfer.files[0]);
        }
    };

    const handleFileChange = (e) => {
        if (e.target.files && e.target.files[0]) {
            setFile(e.target.files[0]);
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!file) return;

        setIsLoading(true);
        try {
            const data = new FormData();
            data.append('Title', formData.title);
            data.append('Description', formData.description);
            data.append('Price', formData.price);
            data.append('Category', formData.category);
            data.append('File', file);

            // Metadata as JSON string if needed, or separate fields
            const metadata = {
                width: formData.width,
                height: formData.height,
                year: formData.year
            };
            data.append('Metadata', JSON.stringify(metadata));

            await artworkService.create(data);
            alert('Artwork uploaded successfully!');
            navigate('/dashboard/artist/artworks');
        } catch (error) {
            console.error("Upload failed", error);
            alert("Failed to upload artwork. Please try again.");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="max-w-3xl mx-auto">
            <div className="mb-8">
                <h1 className="text-2xl font-heading font-bold text-primary">Upload New Artwork</h1>
                <p className="text-textSecondary">Share your masterpiece with the world</p>
            </div>

            <form onSubmit={handleSubmit} className="space-y-8 bg-white p-8 rounded-2xl shadow-sm border border-border">
                {/* Drag & Drop Zone */}
                <div>
                    <label className="block text-sm font-medium text-textSecondary mb-2">Artwork Image</label>
                    <div
                        className={`relative dashed-border border-2 border-dashed rounded-xl p-8 text-center transition-colors ${dragActive ? 'border-primary bg-primary/5' : 'border-gray-300 hover:border-primary'}`}
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
                        label="Price ($)"
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
                    ></textarea>
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
                            <option>Visual Arts</option>
                            <option>Calligraphy</option>
                            <option>Digital Art</option>
                            <option>Sculpture</option>
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

                <div className="flex justify-end pt-4">
                    <Button type="submit" variant="primary" isLoading={isLoading} disabled={!file}>
                        Publish Artwork
                    </Button>
                </div>
            </form>
        </div>
    );
};

export default UploadArtwork;
