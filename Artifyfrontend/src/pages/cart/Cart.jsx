import React from 'react';
import { Link } from 'react-router-dom';
import { Trash2, ArrowRight } from 'lucide-react';
import { useCart } from '../../context/CartContext';
import Button from '../../components/common/Button';

const Cart = () => {
    const { cartItems, removeFromCart, cartTotal } = useCart();

    if (cartItems.length === 0) {
        return (
            <div className="min-h-[60vh] flex flex-col items-center justify-center p-4">
                <div className="w-20 h-20 bg-gray-100 rounded-full flex items-center justify-center mb-6">
                    <Trash2 className="w-8 h-8 text-gray-400" />
                </div>
                <h2 className="text-2xl font-bold text-primary mb-2">Your Cart is Empty</h2>
                <p className="text-textSecondary mb-8">Looks like you haven't added any masterpieces yet.</p>
                <Link to="/marketplace">
                    <Button variant="primary">Start Exploring</Button>
                </Link>
            </div>
        );
    }

    return (
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-12">
            <h1 className="text-3xl font-heading font-bold text-primary mb-8">Shopping Cart</h1>

            <div className="flex flex-col lg:flex-row gap-12">
                {/* Cart Items */}
                <div className="flex-1 space-y-6">
                    {cartItems.map((item) => (
                        <div key={item.id} className="flex gap-4 p-4 bg-white rounded-xl shadow-sm border border-border">
                            <div className="w-24 h-24 rounded-lg overflow-hidden flex-shrink-0 bg-gray-100">
                                <img src={item.image} alt={item.title} className="w-full h-full object-cover" />
                            </div>
                            <div className="flex-1 flex flex-col justify-between">
                                <div className="flex justify-between items-start">
                                    <div>
                                        <h3 className="font-bold text-primary text-lg">{item.title}</h3>
                                        <p className="text-sm text-textSecondary">by {item.artist}</p>
                                    </div>
                                    <button
                                        onClick={() => removeFromCart(item.id)}
                                        className="text-gray-400 hover:text-error transition-colors"
                                    >
                                        <Trash2 className="w-5 h-5" />
                                    </button>
                                </div>
                                <div className="flex justify-between items-center mt-2">
                                    <span className="text-sm text-gray-500">{item.medium}</span>
                                    <span className="font-bold text-lg text-secondary">{item.price}</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>

                {/* Summary */}
                <div className="w-full lg:w-96">
                    <div className="bg-white rounded-xl shadow-lg p-6 border border-border sticky top-24">
                        <h3 className="font-bold text-xl text-primary mb-6">Order Summary</h3>

                        <div className="space-y-4 mb-6">
                            <div className="flex justify-between text-textSecondary">
                                <span>Subtotal</span>
                                <span>${cartTotal}</span>
                            </div>
                            <div className="flex justify-between text-textSecondary">
                                <span>Shipping (Estimate)</span>
                                <span>$50</span>
                            </div>
                            <div className="border-t border-gray-100 pt-4 flex justify-between font-bold text-lg text-primary">
                                <span>Total</span>
                                <span>${cartTotal + 50}</span>
                            </div>
                        </div>

                        <Link to="/checkout">
                            <Button variant="primary" className="w-full">
                                Proceed to Checkout <ArrowRight className="w-4 h-4 ml-2" />
                            </Button>
                        </Link>

                        <div className="mt-4 flex items-center justify-center gap-2 text-xs text-gray-400">
                            <span className="w-2 h-2 rounded-full bg-green-500"></span> Secure Checkout via Stripe
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Cart;
