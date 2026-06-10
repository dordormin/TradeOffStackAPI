import {themes as prismThemes} from 'prism-react-renderer';
import type {Config} from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: 'TradeOffStack Enterprise',
  tagline: 'Documentation technique et standards de développement',
  favicon: 'img/favicon.ico',

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  // Set the production url of your site here
  url: 'https://docs.tradeoffstack.local',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: 'TradeOffStack', // Usually your GitHub org/user name.
  projectName: 'internal-docs', // Usually your repo name.

  onBrokenLinks: 'throw',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'fr',
    locales: ['fr', 'en'],
  },

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
        },
        blog: false,
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    // Replace with your project's social card
    image: 'img/docusaurus-social-card.jpg',
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: 'TradeOffStack',
      logo: {
        alt: 'TradeOffStack Logo',
        src: 'img/logo.svg',
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'tutorialSidebar',
          position: 'left',
          label: 'Documentation Technique',
        },
        {
          href: 'https://github.com/dordormin/TradeOffStackAPI',
          label: 'Dépôt GitHub',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'light',
      links: [
        {
          title: 'Parcours Pédagogique',
          items: [
            {
              label: 'Tutoriels',
              to: '/docs/tutoriels/setup-environnement',
            },
            {
              label: 'Concepts Clés',
              to: '/docs/concepts/iam-et-rbac',
            },
            {
              label: 'Snippets & REX',
              to: '/docs/snippets/client-api-axios',
            },
          ],
        },
        {
          title: 'Interne',
          items: [
            {
              label: 'Portail TradeOffStack',
              href: 'https://tradeoffstack.local',
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} TradeOffStack. Documentation interne confidentielle.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
