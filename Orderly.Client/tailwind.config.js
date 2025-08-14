module.exports = {
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                // Light mode
                lightbg: '#FFFFFF',
                lightfg: '#1F2937',
                lightcard: '#F3F4F6',
                primary: '#3B82F6',
                secondary: '#6366F1',
                accent: '#F59E0B',

                // Dark mode
                darkbg: '#101112',       // very dark carbon gray for the main background
                darkfg: '#E5E7EB',       // text remains light for contrast
                darkcard: '#1A1C1F',     // slightly lighter dark gray for cards/panels
            },
        },
    },
    content: [
        "./Pages/**/*.razor",
        "./Components/**/*.razor",
        "./wwwroot/**/*.html",
        "./**/*.cshtml"
    ],
    plugins: [],
};
