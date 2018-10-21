const path = require('path');
const webpack = require('webpack');

const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    mode: "development",
    entry: {
        'main': './src/index.js'
    },
    output: {
        path: path.resolve(__dirname, 'dist'),
        filename: 'bundle.js',
        publicPath: path.resolve(__dirname, 'dist')
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jquery',
            'window.jQuery': 'jquery',
            Poper: ["poper.js", "default"]
        })
    ],
    module: {
        rules: [
            {
                test: /\.css$/,
                loaders: [
                    "style-loader",
                    "css-loader"
                ]
            },
            {
                test: /.*\.jsx?/,
                exclude: /node_modules/,
                loader: ['babel-loader']
            },
            {
                test: /\.(jpg|jpeg|png|gif|mp3|svg)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        useRelativePath: true,
                        publicPath: '/dist'
                    }
                }]
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        useRelativePath: true,
                        publicPath: '/dist'
                    }
                }]
            }
        ]
    },
    resolve: {
        extensions: ['.js', '.jsx']
    },
};