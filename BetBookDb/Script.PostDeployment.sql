if not exists (select 1 from dbo.[Teams])

begin
    insert into dbo.[Teams] (TeamName, City, Stadium, WinCount, LossCount, DrawCount)
        values ('Bengals', 'Cincinnati', 'Paul Brown Stadium', 0, 0, 0),
               ('Raiders', 'Las Vegas', 'Allegiant Stadium', 0, 0, 0),
               ('Chiefs', 'Kansas City', 'Arrowhead Stadium', 0, 0, 0),
               ('Cowboys', 'Dallas', 'AT&T Stadium', 0, 0, 0),
               ('Panthers', 'Carolina', 'Bank Of America Stadium', 0, 0, 0),
               ('Saints', 'New Orleans', 'Caesars Superdome', 0, 0, 0),
               ('Broncos', 'Denver', 'Mile High Stadium', 0, 0, 0),
               ('Commanders', 'Washington', 'FedEx Field', 0, 0, 0),
               ('Browns', 'Cleveland', 'First Energy Stadium', 0, 0, 0),
               ('Lions', 'Detroit', 'Ford Field', 0, 0, 0),
               ('Patriots', 'New England', 'Gillette Stadium', 0, 0, 0),
               ('Dolphins', 'Miami', 'Hard Rock Stadium', 0, 0, 0),
               ('Bills', 'Buffalo', 'Highmark Stadium', 0, 0, 0),
               ('Packers', 'Green Bay', 'Lambeau Field', 0, 0, 0),
               ('49ers', 'San Francisco', 'Levi''s Stadium', 0, 0, 0),
               ('Eagles', 'Philadelphia', 'Lincoln Financial Field', 0, 0, 0),
               ('Colts', 'Indianapolis', 'Lucas Oil Stadium', 0, 0, 0),
               ('Seahawks', 'Seattle', 'Lumen Field', 0, 0, 0),
               ('Ravens', 'Baltimore', 'M&T Bank Stadium', 0, 0, 0),
               ('Falcons', 'Atlanta', 'Mercedes-Benz Stadium', 0, 0, 0),
               ('Giants', 'New York', 'Met Life Stadium', 0, 0, 0),
               ('Jets', 'New York', 'Met Life Stadium', 0, 0, 0),
               ('Titans', 'Tennessee', 'Nissan Stadium', 0, 0, 0),
               ('Texans', 'Houston', 'NRG Stadium', 0, 0, 0),
               ('Buccaneers', 'Tampa Bay', 'Raymond James Stadium', 0, 0, 0),
               ('Rams', 'Los Angeles', 'SoFi Stadium', 0, 0, 0),
               ('Chargers', 'Los Angeles', 'SoFi Stadium', 0, 0, 0),
               ('Bears', 'Chicago', 'Soldier Field', 0, 0, 0),
               ('Cardinals', 'Arizona', 'State Farm Stadium', 0, 0, 0),
               ('Jaguars', 'Jacksonville', 'TIAA Bank Field', 0, 0, 0),
               ('Vikings', 'Minnesota', 'U.S. Bank Stadium', 0, 0, 0),
               ('Steelers', 'Pittsburgh', 'Heinz Field', 0, 0, 0)

    insert into dbo.[TeamRecords] (TeamId, Wins, Losses, Draws) 
        values (1, '', '', ''), (2, '', '', ''), (3, '', '', ''), (4, '', '', ''),
               (5, '', '', ''), (6, '', '', ''), (7, '', '', ''), (8, '', '', ''),
               (9, '', '', ''), (10, '', '', ''), (11, '', '', ''), (12, '', '', ''),
               (13, '', '', ''), (14, '', '', ''), (15, '', '', ''), (16, '', '', ''),
               (17, '', '', ''), (18, '', '', ''), (19, '', '', ''), (20, '', '', ''),
               (21, '', '', ''), (22, '', '', ''), (23, '', '', ''), (24, '', '', ''),
               (25, '', '', ''), (26, '', '', ''), (27, '', '', ''), (28, '', '', ''),
               (29, '', '', ''), (30, '', '', ''), (31, '', '', ''), (32, '', '', '')

    insert into dbo.[HouseAccount] (AccountBalance)
        values (500000);

end
