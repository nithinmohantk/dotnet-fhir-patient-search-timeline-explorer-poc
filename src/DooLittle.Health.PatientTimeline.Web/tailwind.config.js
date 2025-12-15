/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'healthcare': {
          'blue': '#2563eb',
          'blue-light': '#3b82f6',
          'red': '#dc2626',
          'red-light': '#ef4444',
          'gray': '#f5f5f5',
          'gray-dark': '#6b7280',
        }
      }
    },
  },
  plugins: [],
}