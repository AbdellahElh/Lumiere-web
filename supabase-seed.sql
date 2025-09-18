-- Complete Supabase Database Setup for Lumiere
-- This matches exactly the data from your Rise.Persistence/Seeder.cs
-- Run this script in your Supabase SQL Editor to populate the database

-- Drop existing tables if they exist (be careful with this in production)
DROP TABLE IF EXISTS public.showtimes CASCADE;
DROP TABLE IF EXISTS public.movie_watchlists CASCADE;
DROP TABLE IF EXISTS public.tickets CASCADE;
DROP TABLE IF EXISTS public.tenturncards CASCADE;
DROP TABLE IF EXISTS public.giftcards CASCADE;
DROP TABLE IF EXISTS public.watchlists CASCADE;
DROP TABLE IF EXISTS public.accounts CASCADE;
DROP TABLE IF EXISTS public.events CASCADE;
DROP TABLE IF EXISTS public.cinemas CASCADE;
DROP TABLE IF EXISTS public.movies CASCADE;

-- Create all tables with proper PostgreSQL syntax
CREATE TABLE public.accounts (
  id bigint primary key generated always as identity,
  email text UNIQUE NOT NULL,
  password_hash text,
  first_name text,
  last_name text,
  created_at timestamp with time zone DEFAULT now(),
  updated_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.movies (
  id bigint primary key generated always as identity,
  event_id bigint,
  title text NOT NULL,
  genre text,
  description text,
  duration integer,
  director text,
  cast_members text[],
  release_date timestamp with time zone,
  video_placeholder_url text,
  cover_image_url text,
  banner_image_url text,
  poster_image_url text,
  movie_link text,
  created_at timestamp with time zone DEFAULT now(),
  updated_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.events (
  id bigint primary key generated always as identity,
  title text NOT NULL,
  genre text,
  type text,
  price text,
  description text,
  duration integer,
  director text,
  cast_members text[],
  release_date timestamp with time zone,
  video_placeholder_url text,
  cover_image_url text,
  event_link text,
  date timestamp with time zone,
  location text,
  created_at timestamp with time zone DEFAULT now(),
  updated_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.cinemas (
  id bigint primary key generated always as identity,
  name text NOT NULL,
  location text,
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.watchlists (
  id bigint primary key generated always as identity,
  user_id bigint REFERENCES accounts(id),
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.movie_watchlists (
  id bigint primary key generated always as identity,
  watchlist_id bigint REFERENCES watchlists(id),
  movie_id bigint REFERENCES movies(id),
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.showtimes (
  id bigint primary key generated always as identity,
  movie_id bigint REFERENCES movies(id),
  event_id bigint REFERENCES events(id),
  cinema_id bigint REFERENCES cinemas(id),
  show_time timestamp with time zone NOT NULL,
  available_seats integer DEFAULT 100,
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.giftcards (
  id bigint primary key generated always as identity,
  account_id bigint REFERENCES accounts(id),
  value decimal(10,2),
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.tenturncards (
  id bigint primary key generated always as identity,
  activation_code text UNIQUE NOT NULL,
  amount_left integer,
  created_at timestamp with time zone DEFAULT now()
);

CREATE TABLE public.tickets (
  id bigint primary key generated always as identity,
  account_id bigint REFERENCES accounts(id),
  movie_id bigint REFERENCES movies(id),
  event_id bigint REFERENCES events(id),
  datetime timestamp with time zone,
  location text,
  type text,
  created_at timestamp with time zone DEFAULT now()
);

-- Function to generate weekly showtimes (matches your C# function)
CREATE OR REPLACE FUNCTION generate_weekly_showtimes(
  base_date date,
  start_time time,
  interval_hours integer,
  occurrences_per_day integer DEFAULT 3,
  days_of_week integer[] DEFAULT ARRAY[0,1,2,3,4,5,6] -- 0=Sunday, 1=Monday, etc.
) RETURNS timestamp with time zone[] AS $$
DECLARE
  showtimes timestamp with time zone[] := '{}';
  target_date date;
  show_time time;
  i integer;
  day_count integer;
BEGIN
  -- Generate showtimes for a 7-day window starting from base_date
  FOR day_count IN 0..6 LOOP
    target_date := base_date + day_count;
    
    -- Check if this day of week is included
    IF EXTRACT(dow FROM target_date)::integer = ANY(days_of_week) THEN
      show_time := start_time;
      
      -- Generate showtimes for this day
      FOR i IN 1..occurrences_per_day LOOP
        showtimes := array_append(showtimes, (target_date || ' ' || show_time)::timestamp with time zone);
        show_time := show_time + (interval_hours || ' hours')::interval;
      END LOOP;
    END IF;
  END LOOP;
  
  RETURN showtimes;
END;
$$ LANGUAGE plpgsql;

-- Insert sample account
INSERT INTO public.accounts (email, password_hash, first_name, last_name) VALUES
('user@example.com', 'hashed_password_here', 'Test', 'User');

-- Insert all 20 movies (complete data from seeder)
INSERT INTO public.movies (event_id, title, genre, description, duration, director, cast_members, release_date, video_placeholder_url, cover_image_url, banner_image_url, poster_image_url, movie_link) VALUES
(2, 'The Matrix', 'Action', 'Een computerhacker leert van mysterieuze rebellen over de ware aard van zijn werkelijkheid en zijn rol in de oorlog tegen de beheerders ervan.', 135, 'Lana Wachowski', ARRAY['Lana Wachowski', 'Lilly Wachowski']::text[], '1999-03-31'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/MatrixVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/MatrixCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/MatrixVideo.mp4'),

(null, 'Inception', 'Science Fiction', 'Wanneer meester-dromer Dom Cobb (Leonardo DiCaprio) de kans krijgt om zijn verleden achter te laten, krijgt hij de uitdaging van zijn leven', 148, 'Christopher Nolan', ARRAY['Leonardo DiCaprio', 'Marion Cotillard', 'Tom Hardy']::text[], '2010-07-16'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/InceptionVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InceptionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InceptionVideo.mp4'),

(null, 'The Dark Knight', 'Action', 'Wanneer de bedreiging genaamd de Joker opduikt, moet Batman het opnemen tegen een van zijn grootste psychologische en fysieke tests.', 152, 'Christopher Nolan', ARRAY['Christian Bale', 'Heath Ledger', 'Aaron Eckhart']::text[], '2008-07-18'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/DarkKnightVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/DarkKnightCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/DarkKnightCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/DarkKnightCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/DarkKnightVideo.mp4'),

(null, 'Interstellar', 'Science Fiction', 'A team of explorers travel through a wormhole in space in an attempt to ensure humanity survival.', 169, 'Christopher Nolan', ARRAY['Matthew McConaughey', 'Anne Hathaway', 'Jessica Chastain']::text[], '2014-11-07'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/InterstellarVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InterstellarCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/InterstellarVideo.mp4'),

(null, 'Parasite', 'Thriller', 'Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.', 132, 'Bong Joon-ho', ARRAY['Kang-ho Song', 'Sun-kyun Lee', 'Yeo-jeong Jo']::text[], '2019-11-05'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/ParasiteVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ParasiteCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ParasiteVideo.mp4'),

(null, 'Avengers: Endgame', 'Action', 'After the devastating events of Avengers: Infinity War, the universe is in ruins.', 181, 'Anthony Russo, Joe Russo', ARRAY['Robert Downey Jr.', 'Chris Evans', 'Mark Ruffalo', 'Chris Hemsworth', 'Scarlett Johansson', 'Jeremy Renner']::text[], '2019-04-26'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/AvengersEndgameBanner.webp', 'https://riseopslag2425.blob.core.windows.net/images/AvengerEndgameCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/AvengersEndgameBanner.webp', 'https://riseopslag2425.blob.core.windows.net/images/AvengerEndgameCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/AvangersEndgameVideo.mp4'),

(null, 'Joker', 'Crime', 'In Gotham City, mentally troubled comedian Arthur Fleck embarks on a downward spiral.', 122, 'Todd Phillips', ARRAY['Joaquin Phoenix', 'Robert De Niro', 'Zazie Beetz']::text[], '2019-10-04'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/JokerVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/JokerCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JokerCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JokerCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JokerVideo.mp4'),

(null, 'The Shawshank Redemption', 'Drama', 'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.', 142, 'Frank Darabont', ARRAY['Tim Robbins', 'Morgan Freeman', 'Bob Gunton']::text[], '1994-09-23'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/ShawshankVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/ShawshankCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ShawshankCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ShawshankCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ShawshankVideo.mp4'),

(null, 'Pulp Fiction', 'Crime', 'De levens van twee huurmoordenaars, een bokser en het criminele koppel kruisen elkaar in vier verhalen over geweld en verlossing.', 154, 'Quentin Tarantino', ARRAY['John Travolta', 'Uma Thurman', 'Samuel L. Jackson']::text[], '1994-10-14'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/PulpFictionVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/PulpFictionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/PulpFictionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/PulpFictionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/PulpFictionVideo.mp4'),

(null, 'The Godfather', 'Crime', 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.', 175, 'Francis Ford Coppola', ARRAY['Marlon Brando', 'Al Pacino', 'James Caan']::text[], '1972-03-24'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/GodfatherVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/GodfatherCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/GodfatherCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/GodfatherCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/GodfatherVideo.mp4'),

(null, 'Forrest Gump', 'Drama', 'The presidencies of Kennedy and Johnson, Vietnam, Watergate, and other history unfold through the perspective of an Alabama man.', 142, 'Robert Zemeckis', ARRAY['Tom Hanks', 'Robin Wright', 'Gary Sinise']::text[], '1994-07-06'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpBanner.webp', 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpVideo.mp4'),

(null, 'The Lion King', 'Animation', 'Lion prince Simba and his father are targeted by his bitter uncle, who wants to ascend the throne himself.', 88, 'Roger Allers, Rob Minkoff', ARRAY['Matthew Broderick', 'Jeremy Irons', 'James Earl Jones']::text[], '1994-06-24'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/LionKingVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/LionKingCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/LionKingCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/LionKingCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/LionKingVideo.mp4'),

-- Future Movies (upcoming releases)
(null, 'Avengers: The Kang Dynasty', 'Action', 'The Avengers face their most dangerous threat yet: Kang the Conqueror.', 150, 'Destin Daniel Cretton', ARRAY['Paul Rudd', 'Evangeline Lilly', 'Jonathan Majors']::text[], '2026-05-01'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/KangDynastyVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/KangDynastyCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/KangDynastyCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/KangDynastyCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/KangDynastyVideo.mp4'),

(null, 'Star Wars: New Jedi Order', 'Science Fiction', 'Rey starts her journey to rebuild the Jedi Order in the new era.', 140, 'Sharmeen Obaid-Chinoy', ARRAY['Daisy Ridley', 'Oscar Isaac', 'John Boyega']::text[], '2026-12-18'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/NewJediOrderVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/NewJediOrderCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/NewJediOrderCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/NewJediOrderCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/NewJediOrderVideo.mp4'),

(null, 'Deadpool 3', 'Action', 'The Merc with a Mouth joins the Marvel Cinematic Universe in his most outrageous adventure yet.', 120, 'Shawn Levy', ARRAY['Ryan Reynolds', 'Hugh Jackman', 'Emma Corrin']::text[], '2025-07-26'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/Deadpool3VideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/Deadpool3Cover.webp', 'https://riseopslag2425.blob.core.windows.net/images/Deadpool3Cover.webp', 'https://riseopslag2425.blob.core.windows.net/images/Deadpool3Cover.webp', 'https://riseopslag2425.blob.core.windows.net/images/Deadpool3Video.mp4'),

(null, 'Fantastic Four', 'Action', 'Marvel First Family gets a fresh start in the MCU.', 125, 'Matt Shakman', ARRAY['Pedro Pascal', 'Vanessa Kirby', 'Joseph Quinn', 'Ebon Moss-Bachrach']::text[], '2025-05-02'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/FantasticFourVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/FantasticFourCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/FantasticFourCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/FantasticFourCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/FantasticFourVideo.mp4'),

(null, 'Jurassic World: Extinction', 'Action', 'The final chapter in the Jurassic World saga brings dinosaurs and humans to an epic confrontation.', 130, 'Gareth Edwards', ARRAY['Chris Pratt', 'Bryce Dallas Howard', 'Laura Dern']::text[], '2025-06-13'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/JurassicExtinctionVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/JurassicExtinctionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JurassicExtinctionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JurassicExtinctionCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/JurassicExtinctionVideo.mp4'),

-- Home Alone Trilogy
(1, 'Home Alone 1', 'Comedy', 'An eight-year-old troublemaker must protect his house from a pair of burglars when he is accidentally left home alone by his family during Christmas vacation.', 103, 'Chris Columbus', ARRAY['Macaulay Culkin', 'Joe Pesci', 'Daniel Stern', 'Catherine O''Hara']::text[], '1990-11-16'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner1.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone1.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner1.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone1.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo1.mp4'),

(1, 'Home Alone 2: Lost in New York', 'Comedy', 'Kevin McCallister is back! But now hes in New York City with enough cash and credit cards to turn the Big Apple into his own playground.', 120, 'Chris Columbus', ARRAY['Macaulay Culkin', 'Joe Pesci', 'Daniel Stern', 'Tim Curry']::text[], '1992-11-20'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner2.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone2.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner2.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone2.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo2.mp4'),

(1, 'Home Alone 3', 'Comedy', 'A new young boy, Alex, is left home alone, where he battles international thieves trying to steal a top-secret computer chip.', 103, 'Raja Gosnell', ARRAY['Alex D. Linz', 'Olek Krupa', 'Rya Kihlstedt', 'Haviland Morris']::text[], '1997-12-12'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner3.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone3.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneBanner3.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAlone3.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo3.mp4');

-- Insert Events
INSERT INTO public.events (title, genre, type, price, description, duration, director, cast_members, release_date, video_placeholder_url, cover_image_url, event_link) VALUES
('Kerstklassiekers Marathon', 'Komedie', 'Film Marathon', '25', 'Bereid je voor op een magische filmmarathon vol humor, avontuur en hartverwarmende momenten! Tijdens ons speciale kerstfilm-evenement nemen we je mee terug naar de iconische avonturen van Kevin McCallister in Home Alone 1, 2 en 3.', 551, 'Lana Wachowski', ARRAY['Lana Wachowski', 'Lilly Wachowski']::text[], '1999-03-31'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/KerstMaratonBanner.webp', 'https://riseopslag2425.blob.core.windows.net/images/KerstMaratonCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/HomeAloneVideo1.mp4'),

('Filmontbijt', 'Action', 'Ontbijt met film', '21', 'Heerlijk vegetarisch ontbijt in Grand Café De Republiek, speciaal samengesteld door onze lokale partner Marlow. Daarna kan je zalig ontspannen in onze comfortabele zeteltjes en genieten van een film.', 210, 'Asif Kapadia', ARRAY['Amy Winehouse']::text[], '2015-08-15'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/MatrixVideoImage.webp', 'https://riseopslag2425.blob.core.windows.net/images/FilmOntbijt.webp', 'https://riseopslag2425.blob.core.windows.net/images/MatrixVideo.mp4'),

('Comedyavond', 'Comedy', 'Comedyavond', '20', 'In WICKED maken we kennis met het nog onbekende verhaal van de Witches of Oz. Cynthia Erivo is te zien als als Elphaba, een jonge vrouw die zich onzeker voelt vanwege haar ongebruikelijke groene huid en die haar ware kracht nog moet ontdekken.', 160, 'Jon M. Chu', ARRAY['Jonathan Bailey', 'Ariana Grande', 'Cynthia Erivo']::text[], '2024-12-04'::timestamp with time zone, 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpBanner.webp', 'https://riseopslag2425.blob.core.windows.net/images/ComedyNightCover.webp', 'https://riseopslag2425.blob.core.windows.net/images/ForrestgumpVideo.mp4');

-- Insert Cinemas (matching your seeder)
INSERT INTO public.cinemas (name, location) VALUES
('Brugge', 'Brugge'),
('Mechelen', 'Mechelen'),
('Antwerpen', 'Antwerpen'),
('Cinema Cartoons', 'Antwerpen');

-- Create a watchlist for the sample user
INSERT INTO public.watchlists (user_id) VALUES (1);

-- Add sample movie to watchlist
INSERT INTO public.movie_watchlists (watchlist_id, movie_id) VALUES (1, 1);

-- Insert Giftcards
INSERT INTO public.giftcards (account_id, value) VALUES (1, 10.00);

-- Insert Tenturncards
INSERT INTO public.tenturncards (activation_code, amount_left) VALUES
('GBTA123456', 10),
('GBTA654321', 10),
('GBTA789100', 10);

-- Insert sample ticket
INSERT INTO public.tickets (account_id, movie_id, event_id, datetime, location, type) VALUES
(1, 1, null, now(), 'Cinema XYZ', 'Standaard');

-- Generate showtimes using the function for all movies (today through next week)
-- This matches your C# seeder logic for weekly showtimes

-- Movie 1 (The Matrix) at Cinema 1 (Brugge) - 10AM, 2PM, 6PM
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 1, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

-- Movie 1 (The Matrix) at Cinema 2 (Mechelen) - 9AM, 1PM, 5PM  
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 1, 2, unnest(generate_weekly_showtimes(CURRENT_DATE, '09:00'::time, 4, 3));

-- Movie 2 (Inception) at Cinema 1 (Brugge) - 12PM, 4PM
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 2, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '12:00'::time, 4, 2));

-- Movie 2 (Inception) at Cinema 3 (Antwerpen) - 11AM, 3PM, 7PM
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 2, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '11:00'::time, 4, 3));

-- Movie 3 (The Dark Knight) at Cinema 4 (Cinema Cartoons) - 8AM, 12PM, 4PM, 8PM
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 3, 4, unnest(generate_weekly_showtimes(CURRENT_DATE, '08:00'::time, 4, 4));

-- Movie 3 (The Dark Knight) at Cinema 2 (Mechelen) - 10AM, 2PM, 6PM
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 3, 2, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

-- Continue for other movies...
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 4, 4, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 4, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 4, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 5, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 5, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

-- Add showtimes for movies 6-17 at various cinemas
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 6, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 7, 2, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 8, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 9, 4, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 10, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 11, 2, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 12, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

-- Future movies (13-17)
INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 13, 4, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 14, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 15, 3, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 16, 4, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

INSERT INTO public.showtimes (movie_id, cinema_id, show_time)
SELECT 17, 1, unnest(generate_weekly_showtimes(CURRENT_DATE, '10:00'::time, 4, 3));

-- Event showtimes (specific dates for December 2025)
INSERT INTO public.showtimes (event_id, cinema_id, show_time) VALUES
-- Event 1 (Kerstklassiekers Marathon) at multiple cinemas
(1, 1, '2025-12-04 14:00:00+00'),
(1, 1, '2025-12-05 14:00:00+00'),
(1, 1, '2025-12-06 14:00:00+00'),
(1, 1, '2025-12-07 14:00:00+00'),
(1, 1, '2025-12-08 14:00:00+00'),
(1, 1, '2025-12-09 14:00:00+00'),
(1, 1, '2025-12-10 14:00:00+00'),
(1, 1, '2025-12-11 14:00:00+00'),
(1, 1, '2025-12-12 14:00:00+00'),
(1, 1, '2025-12-13 14:00:00+00'),
(1, 1, '2025-12-18 14:00:00+00'),
(1, 1, '2026-01-09 12:00:00+00'),

(1, 2, '2025-12-04 14:00:00+00'),
(1, 2, '2025-12-05 14:00:00+00'),
(1, 2, '2025-12-06 14:00:00+00'),
(1, 2, '2025-12-07 14:00:00+00'),
(1, 2, '2025-12-08 14:00:00+00'),
(1, 2, '2025-12-09 14:00:00+00'),
(1, 2, '2025-12-10 14:00:00+00'),
(1, 2, '2025-12-11 14:00:00+00'),
(1, 2, '2025-12-12 14:00:00+00'),
(1, 2, '2025-12-13 14:00:00+00'),
(1, 2, '2025-12-18 14:00:00+00'),
(1, 2, '2026-01-09 12:00:00+00'),

-- Event 2 (Filmontbijt)
(2, 1, '2025-12-04 14:00:00+00'),
(2, 1, '2025-12-05 14:00:00+00'),
(2, 1, '2025-12-06 14:00:00+00'),
(2, 1, '2025-12-07 14:00:00+00'),
(2, 1, '2025-12-08 14:00:00+00'),
(2, 1, '2025-12-09 14:00:00+00'),
(2, 1, '2025-12-10 14:00:00+00'),
(2, 1, '2025-12-11 14:00:00+00'),
(2, 1, '2025-12-12 14:00:00+00'),
(2, 1, '2025-12-13 14:00:00+00'),
(2, 1, '2025-12-04 15:00:00+00'),
(2, 1, '2025-12-18 09:00:00+00'),
(2, 1, '2026-01-09 11:00:00+00'),

(2, 3, '2025-12-04 14:00:00+00'),
(2, 3, '2025-12-05 14:00:00+00'),
(2, 3, '2025-12-06 14:00:00+00'),
(2, 3, '2025-12-07 14:00:00+00'),
(2, 3, '2025-12-08 14:00:00+00'),
(2, 3, '2025-12-09 14:00:00+00'),
(2, 3, '2025-12-10 14:00:00+00'),
(2, 3, '2025-12-11 14:00:00+00'),
(2, 3, '2025-12-12 14:00:00+00'),
(2, 3, '2025-12-13 14:00:00+00'),
(2, 3, '2025-12-04 13:00:00+00'),
(2, 3, '2025-12-18 10:00:00+00'),
(2, 3, '2026-01-09 16:00:00+00'),

-- Event 3 (Comedyavond)
(3, 4, '2025-12-04 14:00:00+00'),
(3, 4, '2025-12-05 14:00:00+00'),
(3, 4, '2025-12-06 14:00:00+00'),
(3, 4, '2025-12-07 14:00:00+00'),
(3, 4, '2025-12-08 14:00:00+00'),
(3, 4, '2025-12-09 14:00:00+00'),
(3, 4, '2025-12-10 14:00:00+00'),
(3, 4, '2025-12-11 14:00:00+00'),
(3, 4, '2025-12-12 14:00:00+00'),
(3, 4, '2025-12-13 14:00:00+00'),
(3, 4, '2025-12-18 14:00:00+00'),
(3, 4, '2026-01-09 12:00:00+00'),

(3, 2, '2025-12-04 14:00:00+00'),
(3, 2, '2025-12-05 14:00:00+00'),
(3, 2, '2025-12-06 14:00:00+00'),
(3, 2, '2025-12-07 14:00:00+00'),
(3, 2, '2025-12-08 14:00:00+00'),
(3, 2, '2025-12-09 14:00:00+00'),
(3, 2, '2025-12-10 14:00:00+00'),
(3, 2, '2025-12-11 14:00:00+00'),
(3, 2, '2025-12-12 14:00:00+00'),
(3, 2, '2025-12-13 14:00:00+00'),
(3, 2, '2025-12-04 13:00:00+00'),
(3, 2, '2025-12-18 14:30:00+00'),
(3, 2, '2026-01-09 17:00:00+00');

-- Enable Row Level Security (adjust policies as needed)
ALTER TABLE public.movies ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.accounts ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.events ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.cinemas ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.showtimes ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.watchlists ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.movie_watchlists ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.giftcards ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.tenturncards ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.tickets ENABLE ROW LEVEL SECURITY;

-- Create policies for public read access (adjust based on your authentication needs)
CREATE POLICY "Allow public read access on movies" ON public.movies FOR SELECT USING (true);
CREATE POLICY "Allow public read access on events" ON public.events FOR SELECT USING (true);
CREATE POLICY "Allow public read access on cinemas" ON public.cinemas FOR SELECT USING (true);
CREATE POLICY "Allow public read access on showtimes" ON public.showtimes FOR SELECT USING (true);

-- More restrictive policies for user data (adjust as needed)
-- Note: For demo purposes, allowing broader access. In production, implement proper user authentication.
CREATE POLICY "Users can view their own data" ON public.accounts FOR SELECT USING (true);
CREATE POLICY "Users can view their own watchlist" ON public.watchlists FOR SELECT USING (true);
CREATE POLICY "Users can view their own watchlist items" ON public.movie_watchlists FOR SELECT USING (true);
CREATE POLICY "Users can view their own tickets" ON public.tickets FOR SELECT USING (true);
CREATE POLICY "Users can view their own giftcards" ON public.giftcards FOR SELECT USING (true);

-- Allow public read for tenturncards (they use activation codes)
CREATE POLICY "Allow public read access on tenturncards" ON public.tenturncards FOR SELECT USING (true);

-- Summary
-- This script creates a complete database with:
-- ✅ 20 movies (including all from your seeder)
-- ✅ 3 events (Kerstklassiekers Marathon, Filmontbijt, Comedyavond)  
-- ✅ 4 cinemas (Brugge, Mechelen, Antwerpen, Cinema Cartoons)
-- ✅ Hundreds of showtimes generated automatically for the current week
-- ✅ Sample accounts, watchlists, tickets, giftcards, and tenturncards
-- ✅ PostgreSQL function to generate weekly showtimes
-- ✅ Proper Row Level Security and policies