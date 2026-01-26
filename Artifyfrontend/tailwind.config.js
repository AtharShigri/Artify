/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'art-gold': '#D4AF37', 
        'art-charcoal': '#1A1A1A',
      }
    },
  },
  plugins: [],
}