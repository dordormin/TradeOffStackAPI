import React from 'react';
import Link from '@docusaurus/Link';
import useDocusaurusContext from '@docusaurus/useDocusaurusContext';
import Layout from '@theme/Layout';

function HomepageHeader() {
  const {siteConfig} = useDocusaurusContext();
  return (
    <header className="bg-background border-b border-border py-24">
      <div className="container mx-auto px-4 text-center">
        <h1 className="text-5xl font-extrabold tracking-tight text-foreground sm:text-6xl mb-6">
          TradeOffStack <span className="text-primary">Enterprise</span>
        </h1>
        <p className="mt-4 text-xl text-foreground/60 max-w-2xl mx-auto mb-10">
          {siteConfig.tagline}
        </p>
        <div className="flex justify-center gap-4">
          <Link
            className="inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 bg-primary text-primary-foreground shadow hover:bg-primary/90 h-12 px-8 py-2"
            to="/docs/tutoriels/setup-environnement">
            Démarrer l'Onboarding - 5min ⏱️
          </Link>
          <Link
            className="inline-flex items-center justify-center rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50 border border-border bg-background shadow-sm hover:bg-accent hover:text-accent-foreground h-12 px-8 py-2"
            to="/docs/concepts/iam-et-rbac">
            Concepts d'Architecture
          </Link>
        </div>
      </div>
    </header>
  );
}

export default function Home(): JSX.Element {
  const {siteConfig} = useDocusaurusContext();
  return (
    <Layout
      title={`Accueil | ${siteConfig.title}`}
      description="Documentation interne pour les développeurs TradeOffStack.">
      <HomepageHeader />
      <main className="bg-background flex-grow py-20">
        <div className="container mx-auto px-4 max-w-6xl">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
            {/* Tutoriels */}
            <div className="rounded-xl border border-border bg-card text-card-foreground shadow flex flex-col p-6 transition-all hover:shadow-md hover:border-primary/50">
              <div className="flex flex-col space-y-1.5 mb-4">
                <h3 className="font-semibold leading-none tracking-tight text-2xl">🎓 Tutoriels Pratiques</h3>
              </div>
              <p className="text-sm text-foreground/70 flex-grow mb-6">
                Guide étape par étape : de l'installation Docker à la création de votre première API et de votre premier composant React.
              </p>
              <Link to="/docs/tutoriels/setup-environnement" className="text-sm font-medium text-primary hover:underline mt-auto">
                Suivre les tutoriels →
              </Link>
            </div>

            {/* Concepts */}
            <div className="rounded-xl border border-border bg-card text-card-foreground shadow flex flex-col p-6 transition-all hover:shadow-md hover:border-primary/50">
              <div className="flex flex-col space-y-1.5 mb-4">
                <h3 className="font-semibold leading-none tracking-tight text-2xl">🏛️ Concepts Clés</h3>
              </div>
              <p className="text-sm text-foreground/70 flex-grow mb-6">
                Comprenez le "Pourquoi". Plongez dans notre architecture B2B, le Multi-Tenant, et la gestion des rôles (IAM).
              </p>
              <Link to="/docs/concepts/iam-et-rbac" className="text-sm font-medium text-primary hover:underline mt-auto">
                Lire les cours →
              </Link>
            </div>

            {/* Snippets & REX */}
            <div className="rounded-xl border border-border bg-card text-card-foreground shadow flex flex-col p-6 transition-all hover:shadow-md hover:border-primary/50">
              <div className="flex flex-col space-y-1.5 mb-4">
                <h3 className="font-semibold leading-none tracking-tight text-2xl">💻 Snippets & REX</h3>
              </div>
              <p className="text-sm text-foreground/70 flex-grow mb-6">
                La vraie vie des développeurs : résolutions de bugs, Post-Mortems (Playwright, CORS) et standards de code réutilisables.
              </p>
              <Link to="/docs/snippets/client-api-axios" className="text-sm font-medium text-primary hover:underline mt-auto">
                Explorer la base de code →
              </Link>
            </div>
          </div>
        </div>
      </main>
    </Layout>
  );
}
