create or replace PROCEDURE "PR_MCR_PROCES_VEHICULO" (
    id                  TMCR_VEHICULO.VEHID%TYPE,
    placa               TMCR_VEHICULO.VEHPLACA%TYPE,
    modelo              TMCR_VEHICULO.VEHMODELO%TYPE,
    kilometraje         TMCR_VEHICULO.VEHKILOMETRAJE%TYPE,
    tipo                TMCR_VEHICULO.VEHTIPO%TYPE,
    nomeclaturamotor    TMCR_VEHICULO.VEHNOMECLATURA%TYPE,
    locLatitudVehiculo  TMCR_VEHICULO.VEHLOCLATITUD%TYPE,
    locLongitudVehiculo TMCR_VEHICULO.VEHLOCLONGITUD%TYPE,
    observaciones       TMCR_VEHICULO.VEHOBSERVACIONES%TYPE,
    estado              TMCR_VEHICULO.VEHESTADO%TYPE,
    usuario             TMCR_VEHICULO.VEHSUARIOCREACION%TYPE,
    proceso IN number  --> Busqueda Exacta(1), Insertar(2), Actualizar(3), Borrar(4) Registro, Listar todos(5), Busqueda Generica(6)
) AS
    -- Parametros de control:
    p_numRegistros NUMBER:=0;
    p_mensaje VARCHAR2(255 BYTE):='EN EJECUCION'; -- Se coloca el mensaje de respuesta de la transacion
	exc_NoData EXCEPTION;

    maxid TMCR_VEHICULO.VEHID%TYPE; -- Maximo ID, extraer el consecutivo para la insercion

    rf_cursor_leer SYS_REFCURSOR; -- Cursor que se va a retornar
    
BEGIN
    -- Determinar si el registro existe:
    SELECT
        count(*) INTO p_numRegistros
    FROM  TMCR_VEHICULO
    WHERE UPPER(VEHPLACA) = UPPER(placa);
    dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

    -------------------
	-- BUSQUEDA EXACTA:
	-------------------
    IF(proceso = 1) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_VEHICULO
        WHERE
            UPPER(VEHPLACA) = UPPER(placa)
            AND UPPER(VEHNOMECLATURA) = UPPER(nomeclaturamotor);
        dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

        --  SI SE ENCONTRARON REGISTROS, SE PUEDE MOSTRAR DAT0S
        IF(p_numRegistros > 0) THEN

            --p_mensaje := 'BUSQUEDA EXACTA - RESULTADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

            OPEN rf_cursor_leer FOR
                    SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO
                    WHERE
                        UPPER(VEHPLACA) = UPPER(placa)
                        AND UPPER(VEHNOMECLATURA) = UPPER(nomeclaturamotor);
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESAS KEYS, NO SE PUEDE MOSTRAR DATOS
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

	------------
	-- INSERTAR:
	------------
    IF(proceso = 2) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

		-- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_VEHICULO
        WHERE
            UPPER(VEHPLACA) = UPPER(placa)
            AND UPPER(VEHNOMECLATURA) = UPPER(nomeclaturamotor);

        --  YA HAY UN REGISTRADO CON ESA KEY, NO PUEDE INSERTAR ESE REGISTRO
        IF(p_numRegistros > 0) THEN

            -- p_mensaje := 'INSERT - REGISTRO YA EXISTE';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Fallo';

            OPEN rf_cursor_leer FOR
                SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO
                    WHERE
                        UPPER(VEHPLACA) = UPPER(placa)
                        AND UPPER(VEHNOMECLATURA) = UPPER(nomeclaturamotor);
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, PUEDE PROCEDER A INSERTAR
        ELSE
        
            -- Regla para la primer insercion:
            IF(p_numRegistros = 0) THEN
                SELECT
                    COUNT(*) + 1
                    INTO maxid
                FROM TMCR_VEHICULO;
            ELSE
                SELECT
                    MAX(nvl(VEHID, 0)) + 1
                    INTO maxid
                FROM TMCR_VEHICULO;
            END IF;            

            INSERT INTO TMCR_VEHICULO(VEHID, VEHPLACA, VEHMODELO, VEHKILOMETRAJE, VEHTIPO, VEHNOMECLATURA, VEHLOCLATITUD, VEHLOCLONGITUD, VEHOBSERVACIONES, VEHESTADO, VEHFECHAREGISTRO, VEHSUARIOCREACION)
            VALUES(maxid, placa, modelo, kilometraje, tipo, nomeclaturamotor, locLatitudVehiculo, locLongitudVehiculo, observaciones, estado, SYSDATE(), usuario);
            COMMIT;            
            
            -- p_mensaje := 'INSERT - REGISTRO INSERTADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

            OPEN rf_cursor_leer FOR
                SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO
                    WHERE
                        VEHID = maxid;
            dbms_sql.return_result(rf_cursor_leer);

        END IF;

    END IF;

	-------------
	-- ACTUALIZAR:
	-------------
    IF(proceso = 3) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_VEHICULO
        WHERE
            UPPER(VEHPLACA) = UPPER(placa)
            AND UPPER(VEHNOMECLATURA) = UPPER(nomeclaturamotor);
        dbms_output.put_line(' NUMERO REGISTROS --> ' || p_numRegistros);

        --  SI HAY UN REGISTRADO EJECUTAR ACTUALIZACION DEL REGISTRO
        IF(p_numRegistros > 0) THEN

            UPDATE TMCR_VEHICULO
            SET
                VEHPLACA = placa,
				VEHMODELO = modelo,
                VEHKILOMETRAJE = kilometraje,
                VEHTIPO = tipo,
                VEHNOMECLATURA = nomeclaturamotor,
                VEHLOCLATITUD = locLatitudVehiculo,
                VEHLOCLONGITUD = locLongitudVehiculo,
                VEHOBSERVACIONES = observaciones,
                VEHESTADO = estado                
            WHERE
				VEHID = id;
            COMMIT;

            --p_mensaje := 'UPDATE - REGISTRO ACTUALIZADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';
           
            OPEN rf_cursor_leer FOR
                SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO
                    WHERE
                        VEHID = id;
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, NO PUEDE PROCEDER A ACTUALIZAR
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

	------------
	-- ELIMINAR:
	------------
    IF(proceso = 4) THEN

		-- Cerrar el cursor si sigue abierto:
        IF ( rf_cursor_leer%isopen ) THEN
            CLOSE rf_cursor_leer;
        END IF;

        -- Determinar si el registro existe:
        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_VEHICULO
        WHERE
            VEHID = id;

        --  SI HAY UN REGISTRADO EJECUTAR ACTUALIZACION DEL REGISTRO
        IF(p_numRegistros > 0) THEN

            UPDATE TMCR_VEHICULO
            SET
                VEHESTADO = 'INACTIVO'
            WHERE
				VEHID = id;
            COMMIT;

            -- p_mensaje := 'DELETE - REGISTRO INACTIVADO';
            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso' AND MVIABREVIATURA = 'INACTIVO';
            
            OPEN rf_cursor_leer FOR
                SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO
                    WHERE
                        VEHID = id;
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE UN REGISTRO CON ESA KEY, NO SE PUEDE PROCEDER A INACTIVAR
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

    -------------------
	-- BUSQUEDA TODOS:
	-------------------
    IF(proceso = 5) THEN

        --p_mensaje := 'BUSQUEDA - LISTAR TODOS';
        SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = proceso AND MVIESTADO = 'Exitoso';

        OPEN rf_cursor_leer FOR
                SELECT
                        VEHID,
                        VEHPLACA,
                        VEHMODELO,
                        VEHKILOMETRAJE,
                        VEHTIPO, 
                        VEHNOMECLATURA,
                        VEHLOCLATITUD,
                        VEHLOCLONGITUD,
                        VEHOBSERVACIONES,
                        VEHESTADO,    
                        VEHFECHAREGISTRO,
                        VEHSUARIOCREACION,
                        proceso AS PROCESO,
                        p_mensaje AS MENSAJE
                    FROM  TMCR_VEHICULO;
        dbms_sql.return_result(rf_cursor_leer);

    END IF;

    ---------------------
	-- BUSQUEDA GENERICA:
	---------------------
    IF(proceso = 6) THEN

        SELECT
            count(*) INTO p_numRegistros
        FROM  TMCR_VEHICULO
        WHERE
            UPPER(VEHPLACA) LIKE '%' || UPPER(placa) || '%'
			OR UPPER(VEHMODELO) LIKE '%' || UPPER(modelo) || '%'
            OR UPPER(VEHTIPO) LIKE '%' || UPPER(tipo) || '%'
            OR UPPER(VEHNOMECLATURA) LIKE '%' || UPPER(nomeclaturamotor) || '%'
            OR UPPER(VEHOBSERVACIONES) LIKE '%' || UPPER(observaciones) || '%'
            OR UPPER(VEHESTADO) LIKE '%' || UPPER(estado) || '%'
            OR UPPER(VEHSUARIOCREACION) LIKE '%' || UPPER(usuario) || '%';

        --  SI HAY UN REGISTRADO EN LA BUSQUEDA
        IF(p_numRegistros > 0) THEN

            p_mensaje := 'BUSCAR GENERICO - REGISTRO ENCONTRADOS';

            OPEN rf_cursor_leer FOR
                SELECT
					VEHID,
                    VEHPLACA,
                    VEHMODELO,
                    VEHKILOMETRAJE,
                    VEHTIPO, 
                    VEHNOMECLATURA,
                    VEHLOCLATITUD,
                    VEHLOCLONGITUD,
                    VEHOBSERVACIONES,
                    VEHESTADO,    
                    VEHFECHAREGISTRO,
                    VEHSUARIOCREACION,
                    proceso AS PROCESO,
                    p_mensaje AS MENSAJE
				FROM  TMCR_VEHICULO
                WHERE
                    UPPER(VEHPLACA) LIKE '%' || UPPER(placa) || '%'
                    OR UPPER(VEHMODELO) LIKE '%' || UPPER(modelo) || '%'
                    OR UPPER(VEHTIPO) LIKE '%' || UPPER(tipo) || '%'
                    OR UPPER(VEHNOMECLATURA) LIKE '%' || UPPER(nomeclaturamotor) || '%'
                    OR UPPER(VEHOBSERVACIONES) LIKE '%' || UPPER(observaciones) || '%'
                    OR UPPER(VEHESTADO) LIKE '%' || UPPER(estado) || '%'
                    OR UPPER(VEHSUARIOCREACION) LIKE '%' || UPPER(usuario) || '%';
            dbms_sql.return_result(rf_cursor_leer);

        -- NO EXISTE REGISTROS EN LA BUSQIEDA
        ELSE
            RAISE exc_NoData;
        END IF;

    END IF;

    ---------------
	-- EXCEPCIONES:
	---------------
    EXCEPTION
		WHEN exc_NoData THEN

			IF ( rf_cursor_leer%isopen ) THEN
				CLOSE rf_cursor_leer;
			END IF;

            SELECT
                mviestado ||': ' || mvimensaje
                INTO p_mensaje
            FROM TMCR_MENSAJESVISUALIZACION
                WHERE MVIPROCESO = 6 AND MVIESTADO = 'Fallo';

			OPEN rf_cursor_leer FOR
                SELECT
                    0 AS VEHID,
                    '' AS VEHPLACA,
                    '' AS VEHMODELO,
                    '' AS VEHKILOMETRAJE,
                    '' AS VEHTIPO, 
                    '' AS VEHNOMECLATURA,
                    '' AS VEHLOCLATITUD,
                    '' AS VEHLOCLONGITUD,
                    '' AS VEHOBSERVACIONES,
                    '' AS VEHESTADO,    
                    '' AS VEHFECHAREGISTRO,
                    '' AS VEHSUARIOCREACION,
                    proceso AS PROCESO,
                    p_mensaje AS MENSAJE
                FROM  DUAL;
            dbms_sql.return_result(rf_cursor_leer);

			IF ( rf_cursor_leer%isopen ) THEN
				CLOSE rf_cursor_leer;
            END IF;
END;