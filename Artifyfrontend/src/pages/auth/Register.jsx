import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import SEO from '../../components/common/SEO';
import { ART_CATEGORIES } from '../../constants/categories';
import { Building2, User } from 'lucide-react';

const Register = () => {
    const [formData, setFormData] = useState({
        fullName: '',
        email: '',
        password: '',
        confirmPassword: '',
        category: '',
        role: 'buyer',      // 'buyer' | 'artist'
        userType: 0         // 0 = Individual, 1 = Agency
    });
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const { register } = useAuth();
    const navigate = useNavigate();

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
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
            const { confirmPassword, ...payload } = formData;
            await register(payload);
            navigate('/login');
        } catch (err) {
            setError(err.message || 'Failed to register');
        } finally {
            setIsLoading(false);
        }
    };

    const toggleClass = (active) =>
        `flex items-center justify-center gap-2 p-3 text-center rounded-lg border transition-all cursor-pointer ${
            active
                ? 'bg-secondary/10 border-secondary text-secondary font-bold'
                : 'border-gray-200 text-textSecondary hover:border-gray-300'
        }`;

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
                    {/* Role Selection — determines which endpoint is used */}
                    <div>
                        <p className="text-xs font-semibold text-textSecondary uppercase tracking-wide mb-2">I am a...</p>
                        <div className="grid grid-cols-2 gap-3">
                            <button
                                type="button"
                                onClick={() => setFormData({ ...formData, role: 'buyer', category: '' })}
                                className={toggleClass(formData.role === 'buyer')}
                            >
                                Art Enthusiast
                            </button>
                            <button
                                type="button"
                                onClick={() => setFormData({ ...formData, role: 'artist' })}
                                className={toggleClass(formData.role === 'artist')}
                            >
                                Artist
                            </button>
                        </div>
                    </div>

                    {/* UserType Selection — Individual or Agency (applies to both roles) */}
                    <div>
                        <p className="text-xs font-semibold text-textSecondary uppercase tracking-wide mb-2">Account Type</p>
                        <div className="grid grid-cols-2 gap-3">
                            <button
                                type="button"
                                onClick={() => setFormData({ ...formData, userType: 0 })}
                                className={toggleClass(formData.userType === 0)}
                            >
                                <User className="w-4 h-4" />
                                Individual
                            </button>
                            <button
                                type="button"
                                onClick={() => setFormData({ ...formData, userType: 1 })}
                                className={toggleClass(formData.userType === 1)}
                            >
                                <Building2 className="w-4 h-4" />
                                Agency
                            </button>
                        </div>
                        {formData.userType === 1 && (
                            <p className="mt-1.5 text-xs text-textSecondary">
                                {formData.role === 'artist'
                                    ? 'Register as an artist studio or creative collective.'
                                    : 'Register as a business or creative agency to post projects.'}
                            </p>
                        )}
                    </div>

                    <Input
                        label="Full Name"
                        name="fullName"
                        value={formData.fullName}
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

                    {/* Artist-only: Art Category */}
                    {formData.role === 'artist' && (
                        <div>
                            <label className="block text-sm font-medium text-textSecondary mb-1.5">Art Category</label>
                            <select
                                name="category"
                                value={formData.category}
                                onChange={handleChange}
                                required
                                className="w-full px-4 py-3 rounded-lg border border-border focus:outline-none focus:ring-2 focus:ring-secondary/20 focus:border-secondary transition-all bg-white"
                            >
                                <option value="">Select a Category</option>
                                {ART_CATEGORIES.map((cat) => (
                                    <option key={cat} value={cat}>{cat}</option>
                                ))}
                            </select>
                        </div>
                    )}

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
                    By registering, you agree to our{' '}
                    <Link to="/terms" className="underline">Terms of Service</Link> and{' '}
                    <Link to="/privacy" className="underline">Privacy Policy</Link>.
                </p>
            </div>
        </div>
    );
};

export default Register;
