import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import SEO from '../../components/common/SEO';

const Register = () => {
    const [formData, setFormData] = useState({
        name: '',
        email: '',
        password: '',
        confirmPassword: '',
        role: 'buyer' // or 'artist'
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const { register } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        if (formData.password !== formData.confirmPassword) {
            setError('Passwords do not match');
            return;
        }

        setIsLoading(true);

        try {
            await register(formData);
            navigate('/login');
        } catch (err) {
            setError(err.message || 'Failed to register');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-[80vh] flex items-center justify-center px-4 py-12">
            <SEO title="Register" description="Join Artify as an artist or buyer." />
            <div className="w-full max-w-md bg-white p-8 rounded-2xl shadow-xl border border-border">
                <div className="text-center mb-8">
                    <Link to="/" className="inline-block mb-4">
                        <span className="font-heading font-bold text-3xl text-primary">Artify</span>
                    </Link>
                    <h2 className="text-2xl font-bold text-primary mb-2">Join Artify</h2>
                    <p className="text-textSecondary">Create an account to browse or sell unique art</p>
                </div>

                {error && (
                    <div className="mb-6 p-4 bg-red-50 text-error text-sm rounded-lg border border-red-100">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-5">
                    {/* Role Selection */}
                    <div className="grid grid-cols-2 gap-4 mb-2">
                        <button
                            type="button"
                            onClick={() => setFormData({ ...formData, role: 'buyer' })}
                            className={`p-3 text-center rounded-lg border transition-all ${formData.role === 'buyer' ? 'bg-secondary/10 border-secondary text-secondary font-bold' : 'border-gray-200 text-textSecondary hover:border-gray-300'}`}
                        >
                            Art Enthusiast
                        </button>
                        <button
                            type="button"
                            onClick={() => setFormData({ ...formData, role: 'artist' })}
                            className={`p-3 text-center rounded-lg border transition-all ${formData.role === 'artist' ? 'bg-secondary/10 border-secondary text-secondary font-bold' : 'border-gray-200 text-textSecondary hover:border-gray-300'}`}
                        >
                            Artist
                        </button>
                    </div>

                    <Input
                        label="Full Name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                    />

                    <Input
                        label="Email Address"
                        type="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                    />

                    <Input
                        label="Password"
                        type="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />

                    <Input
                        label="Confirm Password"
                        type="password"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        required
                    />

                    <Button type="submit" variant="primary" className="w-full" isLoading={isLoading}>
                        Create Account
                    </Button>
                </form>

                <p className="mt-8 text-center text-sm text-textSecondary">
                    Already have an account?{' '}
                    <Link to="/login" className="font-medium text-secondary hover:text-primary transition-colors">
                        Sign in
                    </Link>
                </p>

                <p className="mt-4 text-center text-xs text-textSecondary">
                    By registering, you agree to our <Link to="/terms" className="underline">Terms of Service</Link> and <Link to="/privacy" className="underline">Privacy Policy</Link>.
                </p>
            </div>
        </div>
    );
};

export default Register;
