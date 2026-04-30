CREATE TABLE recomendations (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    category TEXT NOT NULL,
    embedding vector(1024) NOT NULL
);

CREATE INDEX idx_recomendations ON recomendations USING HNSW (embedding vector_l2_ops);


CREATE TABLE products (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    category TEXT NOT NULL,
    inventory INTEGER NOT NULL DEFAULT 0,
    summary TEXT NOT NULL,
    description TEXT NOT NULL
                      
);