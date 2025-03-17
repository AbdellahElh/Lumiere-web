/** @type {import('tailwindcss').Config} */
const plugin = require("tailwindcss/plugin");
const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
  content: ["../**/*.{razor,cshtml,razor.cs}", "./wwwroot/index.html"],
  theme: {
    screens: {
      ...defaultTheme.screens,

      "lg-plus": "1450px",
      "mb-plus": "1100px",
      "medium-sc": "950px",
    },

    extend: {
      height: {
        104: "26rem",
      },

      keyframes: {
        slideInFromRight: {
          "0%": { transform: "translateX(100%)" },
          "100%": { transform: "translateX(0)" },
        },
        slideInFromLeft: {
          "0%": { transform: "translateX(-100%)" },
          "100%": { transform: "translateX(0)" },
        },
        slideOutToLeft: {
          "0%": { transform: "translateX(0)" },
          "100%": { transform: "translateX(-100%)" },
        },
        slideOutToRight: {
          "0%": { transform: "translateX(0)" },
          "100%": { transform: "translateX(100%)" },
        },
      },

      animation: {
        slideInFromRight: "slideInFromRight 0.4s ease",
        slideInFromLeft: "slideInFromLeft 0.4s ease",
        slideOutToLeft: "slideOutToLeft 0.4s ease",
        slideOutToRight: "slideOutToRight 0.4s ease",
      },
    },
  },
  plugins: [
    require("@tailwindcss/forms"),
    require("@tailwindcss/aspect-ratio"),
    require("@tailwindcss/typography"),
  ],
};
