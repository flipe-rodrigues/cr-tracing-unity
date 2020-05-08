%% initialization
clear;
clc;

%% change directory
levelsPath = uigetdir();
if levelsPath == 0
    return;
end
levelsDir = dir(levelsPath);
levelsDir = levelsDir([levelsDir.isdir]);
levelsDir = levelsDir(3:end);

%% stuff to fix
brk_expressions = {'",','},'};
tab_expressions = {'\n'};
db_id = 0;

% iterate through levels
n_levels = numel(levelsDir);
for ii = 1 : n_levels
    roomsPath = fullfile(levelsDir(ii).folder,levelsDir(ii).name);
    roomsDir = dir([roomsPath,filesep,'*.json']);
    
    %% iterate through rooms
    n_files = numel(roomsDir);
    for jj = 1 : n_files
        jsonFile = fullfile(roomsDir(jj).folder,roomsDir(jj).name);
        
        readfid = fopen(jsonFile,'r');
        raw = fread(readfid);
        fclose(readfid);
        
        str = char(raw');
        data = jsondecode(str);
        data.map = levelsDir(ii).name;
        data.db_id = ['r',num2str(db_id)];
        if contains(data.map,'Second')
            data.position.z = +2;
        elseif contains(data.map,'Main')
            data.position.z = -1;
        else
            data.position.z = +1;
        end
        fields = fieldnames(data);
        offset = find(strcmpi(fields,'db_id'));
        data = orderfields(data,circshift([7,1,2,3,4,5,6],-offset));

        str = jsonencode(data);
        
        str = insertBefore(str,'"db_id"','\n');
        for kk = 1 : numel(brk_expressions)
            str = insertAfter(str,brk_expressions{kk},'\n');
        end
        for kk = 1 : numel(tab_expressions)
            str = insertAfter(str,tab_expressions{kk},'\t');
        end
        str = strrep(str,str(end-10:end),[str(end-10:end-1),'\n',str(end)]);
        
        savefid = fopen(jsonFile,'w');
        fprintf(savefid,str,'char');
        fclose(savefid);
        
        db_id = db_id + 1;
    end
end