import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';
import Button from '../../components/common/Button';
import Input from '../../components/common/Input';
import { Mail, Lock } from 'lucide-react';
import SEO from '../../components/common/SEO';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [role, setRole] = useState('buyer'); // Default role
    const [error, setError] = useState('');
    const [isLoading, setIsLoading] = useState(false);

    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setIsLoading(true);

        try {
            await login(email, password, role);
            navigate('/');
        } catch (err) {
            setError(err.message || 'Failed to login');
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="min-h-[80vh] flex items-center justify-center px-4">
            <SEO title="Login" text="Sign in to your Artify account." />
            <div className="w-full max-w-md bg-white p-8 rounded-2xl shadow-xl border border-border">
                <div className="text-center mb-8">
                    <Link to="/" className="inline-block mb-4">
                        <span className="font-heading font-bold text-3xl text-primary">Artify</span>
                    </Link>
                    <h2 className="text-2xl font-bold text-primary mb-2">Welcome Back</h2>
                    <p className="text-textSecondary">Enter your details to access your account</p>
                </div>

                {error && (
                    <div className="mb-6 p-4 bg-red-50 text-error text-sm rounded-lg border border-red-100">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-6">
                    {/* Role Selection */}
                    <div className="grid grid-cols-2 gap-4">
                        <button
                            type="button"
                            onClick={() => setRole('buyer')}
                            className={`p-3 text-center rounded-lg border transition-all ${role === 'buyer' ? 'bg-secondary/10 border-secondary text-secondary font-bold' : 'border-gray-200 text-textSecondary hover:border-gray-300'}`}
                        >
                            Art Enthusiast
                        </button>
                        <button
                            type="button"
                            onClick={() => setRole('artist')}
                            className={`p-3 text-center rounded-lg border transition-all ${role === 'artist' ? 'bg-secondary/10 border-secondary text-secondary font-bold' : 'border-gray-200 text-textSecondary hover:border-gray-300'}`}
                        >
                            Artist
                        </button>
                    </div>

                    <Input
                        label="Email Address"
                        type="email"
                        placeholder="you@example.com"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                        className="pl-4"
                    />

                    <div className="space-y-1">
                        <Input
                            label="Password"
                            type="password"
                            placeholder="••••••••"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                        <div className="flex justify-end">
                            <Link to="/forgot-password" className="text-sm text-secondary hover:text-primary transition-colors">
                                Forgot password?
                            </Link>
                        </div>
                    </div>

                    <Button type="submit" variant="primary" className="w-full" isLoading={isLoading}>
                        Sign In
                    </Button>
                </form>

                <p className="mt-8 text-center text-sm text-textSecondary">
                    Don't have an account?{' '}
                    <Link to="/register" className="font-medium text-secondary hover:text-primary transition-colors">
                        Create account
                    </Link>
                </p>

                <div className="mt-6 pt-6 border-t border-gray-100 text-center">
                    <p className="text-xs text-textSecondary">
                        Demo credentials: demo@artify.com / password
                    </p>
                </div>
            </div>
        </div>
    );
};

export default Login;
