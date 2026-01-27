/** @type {import('tailwindcss').Config} */
export default {
    content: [
        "./index.html",
        "./src/**/*.{js,ts,jsx,tsx}",
    ],
    theme: {
        extend: {
            colors: {
                primary: "#1E1E2F", // Deep Charcoal Blue
                secondary: "#6C63FF", // Muted Royal Purple
                accent: "#F4C430", // Soft Gold
                background: "#F8F9FC", // Light Background
                textPrimary: "#111827",
                textSecondary: "#6B7280",
                border: "#E5E7EB",
                success: "#10B981", // Soft Green
                error: "#EF4444", // Muted Red
                info: "#8B5CF6", // Purple
            },
            fontFamily: {
                heading: ["Playfair Display", "serif"],
                body: ["Inter", "sans-serif"],
            },
        },
    },
    plugins: [],
}
