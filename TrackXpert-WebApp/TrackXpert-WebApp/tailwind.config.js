/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{razor,html}",
        "./Components/**/*.{razor,html}",
        "./Layout/**/*.{razor,html}",
        "./wwwroot/index.html",
        './node_modules/preline/dist/*.js'
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('preline/plugin'),
    ],

    darkMode: 'media',
}
